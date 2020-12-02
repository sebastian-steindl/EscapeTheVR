using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 https://www.youtube.com/watch?v=0yHBDZHLRbQ
 */
public class DragObject : MonoBehaviour
{
    private float zCoord;
    public ElementStone element;
    private bool hasGravity;

    public GameObject gameBoard;
    public string programmingElementType;
    public bool IsBeingDragged;

    public DragObject(string programmingElementType)
    {
        this.programmingElementType = programmingElementType;
    }

    public void setElementId(int id) {
        element.id = id;
    }

    private void Awake()
    {
        this.element = ElementStoneFactory.Instance.createElementStone(programmingElementType);
        this.hasGravity = GetComponent<Rigidbody>().useGravity;

        this.gameObject.GetComponent<Renderer>().material = MaterialLoader.Instance.getMaterial(programmingElementType);
    }

    private void OnMouseDown()
    {
        // Remember distance to camera
        this.zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Disable gravity while dragging
        GetComponent<Rigidbody>().useGravity = false;

        // with the GetComponent we can call functions from other scripts
        gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(this);
        FindObjectOfType<Inventar>().setCurrentlySelectedElement(this);
        this.IsBeingDragged = true;
    }

    public void OnMouseUp()
    {
        Debug.Log("DragObject->OnMouseUp  + + + + + + +");

        // Re-enable gravity if object is dropped
        GetComponent<Rigidbody>().useGravity = hasGravity;

        this.onButtonUp();
    }

    private void OnMouseDrag()
    {
        var rigidbody = GetComponent<Rigidbody>();

        Vector3 newObjectPosition = GetMouseWorldPos();
        Vector3 force = newObjectPosition - rigidbody.position;

        // Reset velocity to prevent oscillating
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force * 300, ForceMode.Acceleration);
    }

    
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void disableGravity() {
        hasGravity = false;
        GetComponent<Rigidbody>().useGravity = hasGravity;
    }


    public void enableGravity() {
        hasGravity = true;
        GetComponent<Rigidbody>().useGravity = hasGravity;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void onButtonDown()
    {
        this.IsBeingDragged = true;
    }


    public void onButtonUp()
    {
        Debug.Log("DragObject->onButtonUp");

        this.IsBeingDragged = false;

        var workbench = FindObjectOfType<WorkbenchScript>();
        SlotManager gameBoardSlotManager = gameBoard.GetComponent<GameBoardScript>().GetGameBoard();
        SlotManager wbContainerSM = workbench.containerSlotManager;
        SlotManager wbContentSM = workbench.contentSlotManager;
        var currentSelection = gameBoard.GetComponent<GameBoardScript>().selectedGameObj;
        float closestWbContainerDistance = wbContainerSM.getClostestDistance(currentSelection);
        float closestWbContentDistance = wbContentSM.getClostestDistance(currentSelection);
        float closestWbDistance = closestWbContainerDistance;
        if (closestWbContentDistance < closestWbDistance) closestWbDistance = closestWbContentDistance;
        float closestGameBoardDistance = gameBoardSlotManager.getClostestDistance(currentSelection);

        if (closestGameBoardDistance <= closestWbDistance)
        {
            //Make sure that the latest slot has been set...
            (bool isCloseEnough, Slot closestSlot) = gameBoard.GetComponent<GameBoardScript>().setSelectedElementToSlotIfCloseEnough();

            // Update the current position of the programming elements in the puzzle. 
            List<ElementStone> stones = new List<ElementStone>();
            gameBoardSlotManager.slots.ForEach(el => stones.Add(el == null ? null : el.getElement()));

            gameBoard.GetComponent<GameBoardScript>().puzzle.setUserSolution(stones);

            // Check if currently selected element is close enough for counting as inserted into the slot
            if (isCloseEnough)
            {
                // When snapping is enabled, snap current element to the position.
                if (gameBoard.GetComponent<GameBoardScript>().puzzle.isSnapEnabled())
                {
                    disableGravity();
                    gameObject.transform.position = closestSlot.position;
                }

                // Evaluate board (this should only be necessary when at least the now dropped element is close enougth to a slot...)
                bool evaluated = gameBoard.GetComponent<GameBoardScript>().evaluateBoard();
                Debug.Log("DragObject->onButtonOp->evalBoard: " + evaluated);
            }
            else
            {
                //Reenable gravity
                enableGravity();
            }

        }
        else // we are closer to the workbench, so let the workbench handle it
        {
            //Make sure that the latest slot has been set...
            (bool isCloseEnough, Slot closestSlot) = workbench.setSelectedElementToSlotIfCloseEnough();

            // Check if currently selected element is close enough for counting as inserted into the slot
            if (isCloseEnough)
            {
                // When snapping is enabled, snap current element to the position. => TODO!
                if (gameBoard.GetComponent<GameBoardScript>().puzzle.isSnapEnabled())
                {
                    disableGravity();
                    gameObject.transform.position = closestSlot.position;
                }
            }
            else
            {
                //Reenable gravity
                // Reset elemnts if current object was a Var||Interval on the workbench.
                if (this == workbench.containerSlotManager.slots[0].GetDragObject())
                {
                    enableGravity();
                    workbench.resetContentSlots();
                    workbench.containerSlotManager.slots[0].resetElement();
                }
                else if (workbench.contentSlotManager.Find(this) != -1)
                { //If element is currently set in one of the contentslots, reset only this slot. Matters only if object is on workbench.
                    enableGravity();
                    workbench.contentSlotManager.slots[workbench.contentSlotManager.Find(this)].resetElement();
                }
            }
        }

        // Reset currently selected element in components that keep track of it
        FindObjectOfType<Inventar>().resetCurrentlySelectedElement();
        gameBoard.GetComponent<GameBoardScript>().resetSelectedElement();
    }
}
