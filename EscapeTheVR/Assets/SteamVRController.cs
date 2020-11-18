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

    public Vector3 newHandPosition;
    public Vector3 oldHandPosition;

    public SteamVR_Behaviour_Pose pose;

    // Start is called before the first frame update
    void Start()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        triggerOnOff.AddOnStateDownListener(TriggerPress, handType);
        triggerOnOff.AddOnStateUpListener(TriggerRelease, handType);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Debug.Log(pose.GetAngularVelocity());
        /*
        if (objectInHand)
        {

            var rigidbody = objectInHand.GetComponent<Rigidbody>();

            Vector3 newObjectPosition = this.transform.position;
            Vector3 force = newObjectPosition - rigidbody.position;

            // Reset velocity to prevent oscillating
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(force * 10000, ForceMode.Acceleration);
        }
        */
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
        objectInHand.transform.position = this.transform.position;
        //objectInHand.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void DropObject()
    {
        //objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.SetParent(null);
        objectInHand.GetComponent<Rigidbody>().useGravity = true;
        objectInHand.GetComponent<Rigidbody>().velocity = pose.GetVelocity();
        objectInHand.GetComponent<Rigidbody>().angularVelocity = pose.GetAngularVelocity();
        objectInHand = null;

        
    }

}