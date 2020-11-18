using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    public GameBoard gameBoard;
    public Puzzle puzzle;
    private GameObject selectedGameObj;
    private ElementStone selectedElement;

    public GameObject slotPrefab;
    private List<ElementStone> allElementStones;
    private Slot lastClosest;

    public AudioSource successSound;
    void Start()
    {
        string path = "/Resources/level" + (PlayerPrefs.GetInt("level",-1)!=-1?""+PlayerPrefs.GetInt("level"):"1")+".xml";
        bool enableSnap = PlayerPrefs.GetString("snapEnabled","true").Equals("true");
        (Puzzle puz, Level level) = PuzzleXMLReader.readLevel(path, true);
        //(Puzzle puz, Level level) = PuzzleXMLReader.readLevel("/Resources/level1.xml", true);
        puzzle = puz;

        successSound = GetComponent<AudioSource>();

        gameBoard = new GameBoard(gameObject.transform.position, gameObject.transform.localScale, puzzle);
        gameBoard.initSlots();
        level.puzzleProgrammingElements.ForEach(el => createElementFromPrefab(el));
        gameBoard.slots.ForEach(s => createSlotFromPrefab(s));
    }

    public DragObject createElementFromPrefab(PuzzleProgrammingElement puzzleElement)
    {
        //TODO anpassungen sodass richtig erzeugt wird: Farbe etc.
        GameObject instantiated = Instantiate(getPrefabForPuzzleProgrammingElement(puzzleElement));
        instantiated.transform.position = new Vector3(puzzleElement.positionX, puzzleElement.positionY, puzzleElement.positionZ);
        instantiated.GetComponent<DragObject>().gameBoard = this.gameObject;
        instantiated.GetComponent<DragObject>().setElementId(puzzleElement.id);
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

    public GameBoard GetGameBoard() { return (GameBoard)gameBoard; }

    // Update is called once per frame
    void Update()
    {

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
        
        //Get Distance
        (bool isCloseEnough, Slot closestSlot) = gameBoard.checkIfElementIsPlacedOverASlot(selectedGameObj);
        if (isCloseEnough)
        {
           //If the latest slot is not equal to the current one and is populated by the current element, reset that slot.
            if (lastClosest != null && lastClosest != closestSlot && closestSlot.getElement() != null && closestSlot.getElement().Equals(selectedElement))
                lastClosest.resetElement();

            Debug.Log("***Close enough***");
            Debug.Log("SetSlot: \nClosest: "+closestSlot.position+"\tLast: "+ (lastClosest == null ? new Vector3(-1f, -1f, -1f) : lastClosest.position));
            closestSlot.setElement(selectedElement);
            lastClosest = closestSlot;
        }
        else if (closestSlot.getElement() != null && closestSlot.getElement().Equals(selectedElement)) // if slot isn't close enough but the current element is still set in the slot, remove it.
            closestSlot.resetElement();
        return (isCloseEnough, closestSlot);
    }

    public void registerSelectedElement(GameObject obj, ElementStone element)
    {
        selectedGameObj = obj;
        selectedElement = element;
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
            successSound.Play();
        }
        return puzzleCorrectlySolved;
    }
}
