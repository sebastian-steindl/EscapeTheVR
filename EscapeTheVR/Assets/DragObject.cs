using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 https://www.youtube.com/watch?v=0yHBDZHLRbQ
 */
public class DragObject : MonoBehaviour
{
    //private Vector3 offset;
    private float zCoord;
    private float xCoord;
    public GameObject gameBoard;

    public string programmingElementType;
    private ElementStone element;

    public DragObject(string programmingElementType) {
        this.programmingElementType = programmingElementType;
    }

    private void Awake()
    {
        element = ElementStoneFactory.Instance.createElementStone(programmingElementType);

        gameObject.GetComponent<Renderer>().material = MaterialLoader.Instance.getMaterial(programmingElementType);
        
    }

    private void OnMouseDown()
    {
        //offset = gameObject.transform.position - GetMouseWorldPos();
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // with the GetComponent we can call functions from other scripts
        gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(gameObject, element);
    }

    private void OnMouseUp()
    {
        Debug.Log("DragObject->OnMouseUp  + + + + + + +");
        GameBoard gameBoardEl = gameBoard.GetComponent<GameBoardScript>().GetGameBoard();
        
        // Update the current position of the programming elements in the puzzle. 
        List<ElementStone> stones = new List<ElementStone>();
        gameBoardEl.getSlots().ForEach(el => stones.Add(el==null?null:el.getElement()));
        gameBoardEl.getPuzzle().setUserSolution(stones);
        
        //Check if currently selected element is close enougth for counting as inserted into the slot
        (bool isCloseEnough, Slot closestSlot) = gameBoardEl.checkIfElementIsPlacedOverASlot(gameObject);
        if (isCloseEnough)
        {
            // When snapping is enabled, snap current element to the position. => TODO!
            if (gameBoardEl.getPuzzle().isSnapEnabled()) {
                //gameObject.transform.position = closestSlot.position;
            }

            //Evaluate board (this should only be nesscary when at least the now droped element is close enougth to a slot...)
            Debug.Log("DragObject->OnMouseUp->evalBoard: "+ gameBoard.GetComponent<GameBoardScript>().evaluateBoard());
        }
        else
            closestSlot.resetElement();
        
        //Reset currently selected element
        gameBoard.GetComponent<GameBoardScript>().resetSelectedElement();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos();// + offset;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
