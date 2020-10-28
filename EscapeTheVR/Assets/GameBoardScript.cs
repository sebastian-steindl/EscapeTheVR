using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    public GameBoard gameBoard;
    public Puzzle puzzle;
    private GameObject selectedGameObj;
    private ElementStone selectedElement;

    private List<ElementStone> allElementStones; 
    void Start()
    {
        puzzle = new Puzzle("Hello World", 2);
        ElementStone printStone = new ElementStone(Constants.colorPrint, Constants.descriptionPrintStone);
        VariableStone variableStone = new VariableStone(Constants.colorVar, Constants.descriptionDefault);
        puzzle.setSolution(new List<ElementStone>()
        {
            printStone,
            variableStone
        });
        gameBoard = new GameBoard(gameObject.transform.position,gameObject.transform.localScale, puzzle);
        gameBoard.initSlots();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
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
            closestSlot.setElement(selectedElement);
        }
    }

    public void registerSelectedElement(GameObject obj, ElementStone element) // TODO: Is param GameObject?
    {
        selectedGameObj = obj;
        selectedElement = element;
    }

    public void resetSelectedElement()
    {
        selectedGameObj = null;
        selectedElement = null;
    }
}
