using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintObjectBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var elementStone = other.gameObject.GetComponent<DragObject>()?.element;
        if (elementStone == null) return;

        elementStone.PlayHintSound();

        // Play laughing animation
        var anim = gameObject.GetComponent<Animation>();
        if (!anim.isPlaying)
        {
            anim.Play("Armature|jiggling");
        }
    }
}
