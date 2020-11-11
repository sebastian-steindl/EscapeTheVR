using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    public GameBoard gameBoard;
    public Puzzle puzzle;
    private GameObject selectedGameObj;
    private ElementStone selectedElement;

    public GameObject prefab;
    public GameObject slotPrefab;
    private List<ElementStone> allElementStones; 
    void Start()
    {
        puzzle = new Puzzle("Hello World", 2);
        puzzle.setSolution(new List<programmingElement>()
        {
            programmingElement.elemFuncPrint,
            programmingElement.elemVar
        });
        gameBoard = new GameBoard(gameObject.transform.position,gameObject.transform.localScale, puzzle);
        gameBoard.initSlots();
        (Puzzle puz, Level level) = PuzzleXMLReader.readLevel("/Resources/level1.xml");
        level.puzzleProgrammingElements.ForEach(el => createElementFromPrefab(el));
        gameBoard.getSlots().ForEach(s => createSlotFromPrefab(s));
    }

    public DragObject createElementFromPrefab(PuzzleProgrammingElement puzzleElement)
    {
        //TODO anpassungen sodass richtig erzeugt wird: Farbe etc.
        GameObject instantiated = Instantiate(prefab);
        instantiated.transform.position = new Vector3(puzzleElement.positionX, puzzleElement.positionY, puzzleElement.positionZ);
        return null;
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
        Debug.Log("Enter" + Input.mousePosition.ToString());
    }

    private void OnMouseExit()
    {
        Debug.Log("Exit" + Input.mousePosition.ToString());
    }

    private void OnMouseOver()
    {
        if (!selectedGameObj) return;

        (bool isCloseEnough, Slot closestSlot) = gameBoard.checkIfElementIsPlacedOverASlot(selectedGameObj);

        if(isCloseEnough)
        {
            Debug.Log("***Close enough***");
            Debug.Log(closestSlot.ToString());
            Debug.Log(selectedElement.ToString());
            closestSlot.setElement(selectedElement);
        }
    }

    public void registerSelectedElement(GameObject obj, ElementStone element)
    {
        selectedGameObj = obj;
        selectedElement = element;
    }

    public ElementStone getSelectedElement() {
        return selectedElement;
    }

    public void resetSelectedElement()
    {
        selectedGameObj = null;
        selectedElement = null;
    }

    public bool evaluateBoard() {
        Debug.Log("evaluateBoard called");
        return puzzle.evaluatePuzzle(true);
    }
}
