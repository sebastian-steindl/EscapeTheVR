using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    public GameBoard gameBoard;
    public Puzzle puzzle;
    private GameObject selectedGameObj;
    private ElementStone selectedElement;

    void Start()
    {
        puzzle = new Puzzle();
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
        Debug.Log("Over" + Input.mousePosition.ToString());
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
