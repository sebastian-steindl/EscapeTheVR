using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    public GameObject ControlledObject;

    // Start is called before the first frame update
    void Start()
    {
        this.ControlledObject.GetComponent<Rigidbody>().useGravity = false;
        this.ControlledObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        this.ControlledObject.GetComponent<Rigidbody>().isKinematic = false;
        this.ControlledObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
