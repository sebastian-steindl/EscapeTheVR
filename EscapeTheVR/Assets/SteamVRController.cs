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

    private int actionTimer = 0;
    private bool actionTimerRecently = false;



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
        if(actionTimer > 10)
        {
            actionTimerRecently = false;
            actionTimer = 0;
        }

        actionTimer++;
    }



    /* Actions */

    public void TriggerMeAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        FindObjectOfType<GameBoardScript>().openMenu();
    }

    public void TriggerPressAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!actionTimerRecently)
        {
            if (inventoryOpened)
            {
                TakeObjectFromInventory();

            }
            else if (collidingObject)
            {
                if (GrabObject(collidingObject))
                {
                    objectInHand = collidingObject;
                    if (objectInHand.GetComponent<DragObject>())
                    {
                        objectInHand.GetComponent<DragObject>().onButtonDown();
                    }
                }
            }

            actionTimerRecently = true;
            actionTimer = 0;

        }
        
    }
    public void TriggerReleaseAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!actionTimerRecently)
        {
            if (objectInHand)
            {
                if (DropObject(objectInHand))
                {
                    if (objectInHand.GetComponent<DragObject>())
                    {
                        objectInHand.GetComponent<DragObject>().onButtonUp();
                    }
                    objectInHand = null;
                }
            }

            actionTimerRecently = true;
            actionTimer = 0;
        }
        
    }

    public void InventoryOpenCloseAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!actionTimerRecently)
        {
            if (!inventoryOpened)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
            actionTimerRecently = true;
            actionTimer = 0;
        }
    }

    public void AddToInventoryAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!actionTimerRecently)
        {

            if (objectInHand)
            {
                if (AddObjectToInventory(objectInHand)) objectInHand = null;
            }
            actionTimerRecently = true;
            actionTimer = 0;
        }
        
    }

    public void NextInventoryItem(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!actionTimerRecently)
        {
            if (inventoryOpened)
            {
                CycleRight();
            }
            actionTimerRecently = true;
            actionTimer = 0;
        }
        
    }

    public void LastInventoryItem(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!actionTimerRecently)
        {
            if (inventoryOpened)
            {
                CycleLeft();
            }
            actionTimerRecently = true;
            actionTimer = 0;
        }
    }



    /* Listener */

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            collidingObject = other.gameObject;
        } 
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

        if (gameObject.GetComponent<DragObject>()) {
            if(!gameObject.GetComponent<DragObject>().isLocked) gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.transform.SetParent(this.transform);
        gameObject.transform.position = this.transform.position;

        return true;
    }

    private bool DropObject(GameObject gameObject)
    {
        gameObject.transform.SetParent(null);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        if (gameObject.GetComponent<DragObject>())
        {
            if (!gameObject.GetComponent<DragObject>().isLocked) gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        gameObject.GetComponent<Rigidbody>().velocity = pose.GetVelocity();
        gameObject.GetComponent<Rigidbody>().angularVelocity = pose.GetAngularVelocity();

        return true;
    }

    private bool AddObjectToInventory(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<DragObject>().gameBoard.GetComponent<GameBoardScript>().resetSelectedElement();
        inventory.Add(gameObject);

        return true;
    }

    private bool OpenInventory()
    {
        //If Inventory is empty or object is held, don't open it
        if (inventory.Count == 0 || objectInHand) return false;

        //Make sure inventoryIndex has no over/underflow
        inventoryIndex = myModulo(inventoryIndex, inventory.Count);


        int previousItem = myModulo((inventoryIndex - 1), inventory.Count);
        int nextItem = myModulo((inventoryIndex + 1), inventory.Count);

        if(inventory.Count == 1)
        {
            inventory[inventoryIndex].SetActive(true);
        } 
        else if(inventory.Count == 2)
        {
            inventory[inventoryIndex].SetActive(true);
            inventory[nextItem].SetActive(true);
            inventory[nextItem].GetComponent<Rigidbody>().transform.localPosition += new Vector3(0.4f, 0, 0);
        } 
        else
        {
            inventory[previousItem].SetActive(true);
            inventory[previousItem].GetComponent<Rigidbody>().transform.localPosition -= new Vector3(0.4f, 0, 0);
            inventory[inventoryIndex].SetActive(true);
            inventory[nextItem].SetActive(true);
            inventory[nextItem].GetComponent<Rigidbody>().transform.localPosition += new Vector3(0.4f, 0, 0);
        }

        inventory[inventoryIndex].GetComponent<DragObject>().gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(inventory[inventoryIndex].GetComponent<DragObject>());

        inventoryOpened = true;
        return true;
    }

    private bool CloseInventory()
    {
        int previousItem = myModulo((inventoryIndex - 1), inventory.Count);
        int nextItem = myModulo((inventoryIndex + 1), inventory.Count);

        if (inventory.Count == 1)
        {
            inventory[inventoryIndex].SetActive(false);
        }
        else if (inventory.Count == 2)
        {
            inventory[inventoryIndex].SetActive(false);
            inventory[nextItem].SetActive(false);
            inventory[nextItem].GetComponent<Rigidbody>().transform.localPosition -= new Vector3(0.4f, 0, 0);
        }
        else
        {
            inventory[previousItem].GetComponent<Rigidbody>().transform.localPosition += new Vector3(0.4f, 0, 0);
            inventory[previousItem].SetActive(false);
            inventory[inventoryIndex].SetActive(false);
            inventory[nextItem].GetComponent<Rigidbody>().transform.localPosition -= new Vector3(0.4f, 0, 0);
            inventory[nextItem].SetActive(false);
        }

        inventory[inventoryIndex].GetComponent<DragObject>().gameBoard.GetComponent<GameBoardScript>().resetSelectedElement();

        inventoryOpened = false;
        return false;
    }

    
    private bool CycleRight()
    {
        CloseInventory();

        inventoryIndex = myModulo(++inventoryIndex, inventory.Count);

        OpenInventory();

        return true;
    }

    private bool CycleLeft()
    {
        CloseInventory();

        inventoryIndex = myModulo(--inventoryIndex, inventory.Count);

        OpenInventory();

        return true;
    }

    private bool TakeObjectFromInventory()
    {
        if (!inventoryOpened) return false;
        if (GrabObject(inventory[inventoryIndex]))
        {
            CloseInventory();

            objectInHand = inventory[inventoryIndex];
            inventory.Remove(objectInHand);
            objectInHand.SetActive(true);
            objectInHand.GetComponent<DragObject>().onButtonDown();
        }

        return true;
    }

    private int myModulo(int number, int modulo)
    {
        return (number % modulo + modulo) % modulo;
    }

}