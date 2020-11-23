using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVRController : MonoBehaviour
{
    /* Variables */

    /* Button Variables */
    public SteamVR_Action_Boolean triggerButton;
    public SteamVR_Action_Boolean inventoryButton;
    public SteamVR_Action_Boolean addToInventoryButton;
    public SteamVR_Action_Boolean nextInventoryItemButton;
    public SteamVR_Action_Boolean lastInventoryItemButton;
    public SteamVR_Action_Boolean menuButton;

    /* Steam VR Variables */
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Input_Sources handType;

    /* Inventory */
    public List<GameObject> inventory;
    public bool inventoryOpened = false;
    public int inventoryIndex = 0;

    /* Object Variables */
    public GameObject collidingObject;
    public GameObject objectInHand;
    

    



    /* Unity Methods */

    void Start()
    {
        inventory = new List<GameObject>();

        pose = GetComponent<SteamVR_Behaviour_Pose>();

        //Trigger Press Actions
        triggerButton.AddOnStateDownListener(TriggerPressAction, handType);
        triggerButton.AddOnStateUpListener(TriggerReleaseAction, handType);

        //Inventory Open Action
        inventoryButton.AddOnStateDownListener(InventoryOpenCloseAction, handType);

        //Inventory Add Item Action
        addToInventoryButton.AddOnStateDownListener(AddToInventoryAction, handType);

        //Inventory Cicle Actions
        nextInventoryItemButton.AddOnStateDownListener(NextInventoryItem, handType);
        lastInventoryItemButton.AddOnStateDownListener(LastInventoryItem, handType);

        //Open Menu
        menuButton.AddOnStateDownListener(TriggerMeAction, handType);
    }
    
    void FixedUpdate()
    {

    }



    /* Actions */

    public void TriggerMeAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        FindObjectOfType<GameBoardScript>().openMenu();
    }

    public void TriggerPressAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (inventoryOpened)
        {
            TakeObjectFromInventory();
        }
        if (collidingObject)
        {
            if (GrabObject(collidingObject))
            {
                objectInHand = collidingObject;
                registerElementAsActive();
                //objectInHand.GetComponent<DragObject>().onButtonDown();
            }
        }
    }
    public void TriggerReleaseAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (objectInHand)
        {
            if (DropObject(objectInHand))
            {
                //objectInHand.GetComponent<DragObject>().onButtonUp();
                objectInHand = null;
            }
        }
    }

    public void InventoryOpenCloseAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!inventoryOpened)
        {
            OpenInventory();
            
        } else
        {
            CloseInventory();
        }
    }

    public void AddToInventoryAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (objectInHand)
        {
            if (AddObjectToInventory(objectInHand)) objectInHand = null;
        }
    }

    public void NextInventoryItem(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (inventoryOpened)
        {
            CycleRight();
        }
    }

    public void LastInventoryItem(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (inventoryOpened)
        {
            CycleLeft();
        }
    }



    /* Listener */

    public void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = other.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        collidingObject = null;
    }



    /* Internal Methods */

    private bool GrabObject(GameObject gameObject)
    {
        //Remove Constraints needed for mouse
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.transform.SetParent(this.transform);
        gameObject.transform.position = this.transform.position;

        return true;
    }

    private bool DropObject(GameObject gameObject)
    {
        gameObject.transform.SetParent(null);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<Rigidbody>().velocity = pose.GetVelocity();
        gameObject.GetComponent<Rigidbody>().angularVelocity = pose.GetAngularVelocity();
        

        return true;
    }

    private bool AddObjectToInventory(GameObject gameObject)
    {
        gameObject.SetActive(false);
        inventory.Add(gameObject);

        return true;
    }

    private bool OpenInventory()
    {
        //If Inventory is empty, don't open it
        if (inventory.Count == 0 || objectInHand) return false;


        if (inventoryIndex >= inventory.Count) inventoryIndex = 0;

        inventory[inventoryIndex].SetActive(true);

        inventoryOpened = true;
        return true;
    }

    private bool CloseInventory()
    {
        inventory[inventoryIndex].SetActive(false);

        inventoryOpened = false;
        return false;
    }

    
    private bool CycleRight()
    {
        inventory[inventoryIndex].SetActive(false);

        if (++inventoryIndex >= inventory.Count) inventoryIndex = 0;

        inventory[inventoryIndex].SetActive(true);

        return true;
    }

    private bool CycleLeft()
    {
        inventory[inventoryIndex].SetActive(false);

        if (--inventoryIndex < 0) inventoryIndex = (inventory.Count - 1);

        inventory[inventoryIndex].SetActive(true);

        return true;
    }

    private bool TakeObjectFromInventory()
    {
        if (GrabObject(inventory[inventoryIndex]))
        {
            CloseInventory();

            objectInHand = inventory[inventoryIndex];
            inventory.Remove(objectInHand);
            objectInHand.SetActive(true);
            registerElementAsActive();
        }

        return true;
    }

    private void registerElementAsActive()
    {
        // always call this function when the player has an element in his hand
        // e.g. inventory, picking up
        var gameboard = objectInHand.GetComponent<DragObject>().gameBoard;
        gameboard.GetComponent<GameBoardScript>().registerSelectedElement(objectInHand.GetComponent<DragObject>());
    }

}