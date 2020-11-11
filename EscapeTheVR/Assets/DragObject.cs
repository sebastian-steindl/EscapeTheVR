using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 https://www.youtube.com/watch?v=0yHBDZHLRbQ
 */
public class DragObject : MonoBehaviour
{
    private float zCoord;
    private ElementStone element;
    private bool hasGravity;

    public GameObject gameBoard;
    public string programmingElementType;

    public DragObject(string programmingElementType)
    {
        this.programmingElementType = programmingElementType;
    }

    private void Awake()
    {
        element = ElementStoneFactory.Instance.createElementStone(programmingElementType);

        gameObject.GetComponent<Renderer>().material = MaterialLoader.Instance.getMaterial(programmingElementType);
    }

    private void OnMouseDown()
    {
        // Remember distance to camera
        this.zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Disable gravity while dragging
        this.hasGravity = GetComponent<Rigidbody>().useGravity;
        GetComponent<Rigidbody>().useGravity = false;

        // with the GetComponent we can call functions from other scripts
        gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(gameObject, element);
    }

    private void OnMouseUp()
    {
        Debug.Log("DragObject->OnMouseUp  + + + + + + +");

        // Re-enable gravity if object is dropped
        GetComponent<Rigidbody>().useGravity = this.hasGravity;

        GameBoard gameBoardEl = gameBoard.GetComponent<GameBoardScript>().GetGameBoard();

        // Update the current position of the programming elements in the puzzle. 
        List<ElementStone> stones = new List<ElementStone>();
        gameBoardEl.getSlots().ForEach(el => stones.Add(el == null ? null : el.getElement()));
        gameBoardEl.getPuzzle().setUserSolution(stones);

        // Check if currently selected element is close enough for counting as inserted into the slot
        (bool isCloseEnough, Slot closestSlot) = gameBoardEl.checkIfElementIsPlacedOverASlot(gameObject);
        if (isCloseEnough)
        {
            // When snapping is enabled, snap current element to the position. => TODO!
            if (gameBoardEl.getPuzzle().isSnapEnabled())
            {
                gameObject.transform.position = closestSlot.position;
            }

            // Evaluate board (this should only be necessary when at least the now dropped element is close enougth to a slot...)
            Debug.Log("DragObject->OnMouseUp->evalBoard: " + gameBoard.GetComponent<GameBoardScript>().evaluateBoard());
        }
        else
        {
            closestSlot.resetElement();
        }

        // Reset currently selected element
        gameBoard.GetComponent<GameBoardScript>().resetSelectedElement();
    }

    private void OnMouseDrag()
    {
        var rigidbody = GetComponent<Rigidbody>();

        Vector3 newObjectPosition = GetMouseWorldPos();
        Vector3 force = newObjectPosition - rigidbody.position;

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force * 100, ForceMode.Acceleration);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
