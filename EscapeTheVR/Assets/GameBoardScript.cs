﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    public SlotManager gameBoard;
    public Puzzle puzzle;
    public GameObject selectedGameObj;
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

        gameBoard = new SlotManager(gameObject.transform.position, gameObject.transform.localScale, puzzle.getNumberOfSlots());
        gameBoard.initSlots();

        level.puzzleProgrammingElements.ForEach(el => createElementFromPrefab(el));
        gameBoard.slots.ForEach(s => createSlotFromPrefab(s));
    }

    public DragObject createElementFromPrefab(PuzzleProgrammingElement puzzleElement)
    {
        //TODO anpassungen sodass richtig erzeugt wird: Farbe etc.
        GameObject instantiated = Instantiate(getPrefabForPuzzleProgrammingElement(puzzleElement));
        instantiated.transform.position = new Vector3(puzzleElement.positionX, puzzleElement.positionY, puzzleElement.positionZ);
        instantiated.GetComponent<DragObject>().gameBoard = gameObject;
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

    public SlotManager GetGameBoard() { return (SlotManager)gameBoard; }

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


        return gameBoard.handlesElementToSlotRelation(selectedGameObj, selectedElement);
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
