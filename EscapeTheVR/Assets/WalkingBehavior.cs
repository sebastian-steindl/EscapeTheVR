using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        var anim = gameObject.GetComponent<Animation>();
        anim.Play("Armature|walking");
    }
}
