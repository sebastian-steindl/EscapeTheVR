using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/*
 https://www.youtube.com/watch?v=0yHBDZHLRbQ
 */
public class DragObject : MonoBehaviour
{
    //VR
    public SteamVR_Action_Boolean triggerOnOff;
    public SteamVR_Input_Sources handType;

    private void Start()
    {
        triggerOnOff.AddOnStateDownListener(TriggerDown, handType);
        triggerOnOff.AddOnStateUpListener(TriggerUp, handType);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
    }


    private float zCoord;
    public ElementStone element;
    private bool hasGravity;

    public GameObject gameBoard;
    public string programmingElementType;

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
        gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(gameObject, element);

        FindObjectOfType<Inventar>().setCurrentlySelectedElement(this);
    }

    private void OnMouseUp()
    {
        Debug.Log("DragObject->OnMouseUp  + + + + + + +");

        // Re-enable gravity if object is dropped
        GetComponent<Rigidbody>().useGravity = this.hasGravity;

        GameBoard gameBoardEl = gameBoard.GetComponent<GameBoardScript>().GetGameBoard();

        //Make shure that the latest slot has been set...
        (bool isCloseEnough, Slot closestSlot) = gameBoard.GetComponent<GameBoardScript>().setSelectedElementToSlotIfCloseEnough();

        // Update the current position of the programming elements in the puzzle. 
        List<ElementStone> stones = new List<ElementStone>();
        gameBoardEl.slots.ForEach(el => stones.Add(el == null ? null : el.getElement()));
        gameBoardEl.activePuzzle.setUserSolution(stones);

        // Check if currently selected element is close enough for counting as inserted into the slot
        if (isCloseEnough)
        {
            // When snapping is enabled, snap current element to the position. => TODO!
            if (gameBoardEl.activePuzzle.isSnapEnabled())
            {
                disableGravity();
                gameObject.transform.position = closestSlot.position;
            }

            // Evaluate board (this should only be necessary when at least the now dropped element is close enougth to a slot...)
            Debug.Log("DragObject->OnMouseUp->evalBoard: " + gameBoard.GetComponent<GameBoardScript>().evaluateBoard());
        }
        else
        {
            // Reset slot
            closestSlot.resetElement();

            //Reenable gravity
            enableGravity();

        }

        // Reset currently selected element
        FindObjectOfType<Inventar>().resetCurrentlySelectedElement();
        gameBoard.GetComponent<GameBoardScript>().resetSelectedElement();
    }

    private void OnMouseDrag()
    {
        var rigidbody = GetComponent<Rigidbody>();

        Vector3 newObjectPosition = GetMouseWorldPos();
        Vector3 force = newObjectPosition - rigidbody.position;

        // Reset velocity to prevent oscillating
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force * 100, ForceMode.Acceleration);
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }


    public void enableGravity() {
        hasGravity = true;
        GetComponent<Rigidbody>().useGravity = hasGravity;
        //GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 9.81f, 0.0f);
    }
}
