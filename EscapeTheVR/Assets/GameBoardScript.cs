using System.Collections;
using System.Collections.Generic;

using TMPro;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBoardScript : MonoBehaviour
{
    public SlotManager gameBoard;
    public Puzzle puzzle;
    /* Shared variable
     * These are also used for the workbench*/
    public DragObject selectedGameObj;
    private ElementStone selectedElement;
    /* End shared variable*/
    public GameObject slotPrefab;
    private List<ElementStone> allElementStones;
    private Slot lastClosest;

    void Start()
    {
        Level level = LevelManager.Instance().getLevelById(PlayerPrefs.GetInt(Constants.playerPrefsLevel,1));
        puzzle = PuzzleXMLReader.createLevel(level);

        gameBoard = new SlotManager(gameObject.transform.position, gameObject.transform.localScale, puzzle.getNumberOfSlots());
        gameBoard.initSlots();

        //create workbench
        var workbench = Instantiate(Resources.Load("Workbench", typeof(GameObject)) as GameObject);
        workbench.transform.parent = this.transform.parent;
        workbench.transform.position = new Vector3(-1.5f,3.29f,-4.0f); //TODO: Set as values in constant class.
        foreach (PuzzleProgrammingElement el in level.puzzleProgrammingElements)
        {
            createElementFromPrefab(el);
        }
        gameBoard.slots.ForEach(s => createSlotFromPrefab(s));
        LevelStartStopHandler.Instance.StartLevel(level.levelId);
        GameObject.Find("Lieferumfang-Text").GetComponent<TextMeshPro>().text = LevelManager.Instance().getCurrentLevelElements();
    }

    public DragObject createElementFromPrefab(PuzzleProgrammingElement puzzleElement)
    {
        //TODO anpassungen sodass richtig erzeugt wird: Farbe etc.
        GameObject instantiated = Instantiate(getPrefabForPuzzleProgrammingElement(puzzleElement));
        instantiated.transform.position = new Vector3(puzzleElement.positionX, puzzleElement.positionY, puzzleElement.positionZ);
        instantiated.GetComponent<DragObject>().gameBoard = gameObject;
        instantiated.GetComponent<DragObject>().setElementId(puzzleElement.id);

        if (puzzleElement.text!= null && puzzleElement.text.Length>0) 
            instantiated.GetComponent<DragObject>().element.descriptionText= puzzleElement.text;
        
        return null;
    }

    private GameObject getPrefabForPuzzleProgrammingElement(PuzzleProgrammingElement puzzleProgrammingElement)
    {

        switch (puzzleProgrammingElement.type)
        {
            case "print()":
                return Resources.Load("prefabPrintElement", typeof(GameObject)) as GameObject;
            case "variable":
                return Resources.Load("prefabVarElement", typeof(GameObject)) as GameObject;
            case "number":
                return Resources.Load("prefabNumberElement", typeof(GameObject)) as GameObject;
            case "text":
                return Resources.Load("prefabTextElement", typeof(GameObject)) as GameObject;
            case "bool":
                return Resources.Load("prefabBoolElement", typeof(GameObject)) as GameObject;
            case "for":
                return Resources.Load("prefabForElement", typeof(GameObject)) as GameObject;
            case "while":
                return Resources.Load("prefabWhileElement", typeof(GameObject)) as GameObject;
            case "==":
                return Resources.Load("prefabEqualsElement", typeof(GameObject)) as GameObject;
            case "&&":
                return Resources.Load("prefabAndElement", typeof(GameObject)) as GameObject;
            case "||":
                return Resources.Load("prefabOrElement", typeof(GameObject)) as GameObject;
            case "!":
                return Resources.Load("prefabNotElement", typeof(GameObject)) as GameObject;
            case "interval":
                return Resources.Load("prefabIntervalElement", typeof(GameObject)) as GameObject;
            case "end":
                return Resources.Load("prefabEndElement", typeof(GameObject)) as GameObject;
            default:
                return Resources.Load("prefabVarElement", typeof(GameObject)) as GameObject;
        }
    }

    public Slot createSlotFromPrefab(Slot slot)
    {
        GameObject instantiated = Instantiate(slotPrefab);
        instantiated.transform.position = slot.position;
        return null;
    }

    public SlotManager GetGameBoard() { return gameBoard; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f1"))
            openMenu();
    }

    private void OnMouseEnter()
    {
        //Do we want to light up the GameBoard when programmingElement enters 
    }

    private void OnMouseExit()
    {
    }

    private void OnMouseOver()
    {
        if (!selectedGameObj) return;

        setSelectedElementToSlotIfCloseEnough();
    }

    /// <summary>
    /// This function sets the currently selected object to the closest slot, if it is close enough.
    /// Attention: It also resets the current object from the slot, if it is still set eventhogh the object isn't close enough.
    /// </summary>
    /// <returns></returns>
    public (bool isCloseEnough, Slot closestSlot) setSelectedElementToSlotIfCloseEnough() {
        if (!selectedGameObj)
            return (false, null);


        return gameBoard.handlesElementToSlotRelation(selectedGameObj);
    }

    public void registerSelectedElement(DragObject obj)
    {
        selectedGameObj = obj;
        selectedElement = obj.element;
        if(puzzle.isSolved)
            GameObject.Find("Code-Text").GetComponent<TextMeshPro>().text = puzzle.createCodeText(obj.element.id);
    }

    public ElementStone getSelectedElement()
    {
        return selectedElement;
    }

    public void resetSelectedElement()
    {
        selectedGameObj = null;
        selectedElement = null;
    }

    public bool evaluateBoard()
    {
        bool puzzleCorrectlySolved = puzzle.evaluatePuzzle();
        if (puzzleCorrectlySolved)
        {
            //LevelManager.Instance().NextLevel();
            handlePuzzleSolved();
        }
        return puzzleCorrectlySolved;
    }

    private void handlePuzzleSolved()
    {
        LevelStartStopHandler.Instance.StopLevel(LevelManager.Instance().getCurrentLevel().levelId);
        AudioManager.Instance.playSuccess();
        GameObject.Find("Code-Text").GetComponent<TextMeshPro>().text = puzzle.createCodeText();
        GameObject.Find("Output-Text").GetComponent<TextMeshPro>().text = puzzle.output;
    }

    public void openMenu() {
        Debug.Log("GameBoardScript:openMenu called!");
        SceneManager.LoadScene("Menu");
    }
}
