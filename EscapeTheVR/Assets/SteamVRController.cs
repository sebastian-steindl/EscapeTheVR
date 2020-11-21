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

    /* Steam VR Variables */
    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Input_Sources handType;

    /* Mixed Variables */
    public GameObject collidingObject;
    public GameObject objectInHand;
    public List<GameObject> inventory;
    public bool inventoryOpened = false;

    



    /* Unity Methods */

    void Start()
    {
        inventory = new List<GameObject>();

        pose = GetComponent<SteamVR_Behaviour_Pose>();

        //Trigger Press Action
        triggerButton.AddOnStateDownListener(TriggerPressAction, handType);
        triggerButton.AddOnStateUpListener(TriggerReleaseAction, handType);

        //Menu Open Action
        inventoryButton.AddOnStateDownListener(InventoryOpenCloseAction, handType);

        //Menu Add Item Action
        addToInventoryButton.AddOnStateDownListener(AddToInventoryAction, handType);
    }
    
    void FixedUpdate()
    {

    }



    /* Actions */

    public void TriggerPressAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (inventoryOpened)
        {
            if(inventory.Count > 0)
            {
                if (GrabObject(inventory[0]))
                {
                    objectInHand = inventory[0];
                    inventory.Remove(objectInHand);
                    objectInHand.SetActive(true);
                    inventoryOpened = false;
                }
                    
            }
        }
        if (collidingObject)
        {
            if (GrabObject(collidingObject)) objectInHand = collidingObject;
        }
    }
    public void TriggerReleaseAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (objectInHand)
        {
            if (DropObject(objectInHand)) objectInHand = null;
        }
    }

    public void InventoryOpenCloseAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!inventoryOpened)
        {
            //Open Inventory
            Debug.Log("Open Inventory");
            inventoryOpened = true;
            
        } else
        {
            //Close Inventory
            Debug.Log("Close Inventory");
            inventoryOpened = false;
        }
    }

    public void AddToInventoryAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (objectInHand)
        {
            if (AddObjectToInventory(objectInHand)) objectInHand = null;
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

}