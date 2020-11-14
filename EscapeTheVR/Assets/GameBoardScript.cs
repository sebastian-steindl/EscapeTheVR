﻿using System.Collections;
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
        (Puzzle puz, Level level) = PuzzleXMLReader.readLevel("/Resources/level1.xml", true);
        puzzle = puz;
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
        Debug.Log("Enter" + Input.mousePosition.ToString());
    }

    private void OnMouseExit()
    {
        Debug.Log("Exit" + Input.mousePosition.ToString());
    }

    private void OnMouseOver()
    {
        if (!selectedGameObj) return;

        setSelectedElementToSlotIfCloseEnough();
    }

    public (bool isCloseEnough, Slot closestSlot) setSelectedElementToSlotIfCloseEnough() {
        if (!selectedGameObj)
            return (false, null);
        
        //Get Distance
        (bool isCloseEnough, Slot closestSlot) = gameBoard.checkIfElementIsPlacedOverASlot(selectedGameObj);
        if (isCloseEnough)
        {

            Debug.Log("***Close enough***");
            //Debug.Log("SetSlot: \nClosest: "+closestSlot.position+"\tLast: "+ (last == null ? new Vector3(-1f, -1f, -1f) : last.position));
            closestSlot.setElement(selectedElement);
        }
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
        Debug.Log("evaluateBoard called");
        return puzzle.evaluatePuzzle(true);
    }
}
