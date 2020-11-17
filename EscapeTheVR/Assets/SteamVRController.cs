using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVRController : MonoBehaviour
{
    //SteamVR Action
    public SteamVR_Action_Boolean triggerOnOff;
    public SteamVR_Input_Sources handType;

    public GameObject collidingObject;
    public GameObject objectInHand;


    // Start is called before the first frame update
    void Start()
    {
        triggerOnOff.AddOnStateDownListener(TriggerPress, handType);
        triggerOnOff.AddOnStateUpListener(TriggerRelease, handType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void TriggerPress(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (collidingObject)
        {
            GrabObject();
        }
    }
    public void TriggerRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (objectInHand)
        {
            DropObject();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

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

    private void GrabObject()
    {
        objectInHand = collidingObject;
        objectInHand.transform.SetParent(this.transform);
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void DropObject()
    {
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.SetParent(null);
    }
}
