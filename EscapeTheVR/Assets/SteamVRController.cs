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

    public Vector3 force;


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
        //objectInHand.transform.position = this.transform.position;
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void DropObject()
    {

        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.SetParent(null);
        objectInHand.GetComponent<Rigidbody>().useGravity = true;

        var rigidbody = objectInHand.GetComponent<Rigidbody>();

        force = this.transform.position - rigidbody.position;

        // Reset velocity to prevent oscillating
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(force * 100, ForceMode.Acceleration);
        

        //objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        //objectInHand.transform.SetParent(null);
        //objectInHand.GetComponent<Rigidbody>().useGravity = true;
        //objectInHand.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //objectInHand.GetComponent<Rigidbody>().AddForce(force * 100, ForceMode.Acceleration);
    }

}