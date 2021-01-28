using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HintObjectBehavior : MonoBehaviour
{
    private List<DateTime> lastPlayTime = new List<DateTime>();

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
        this.lastPlayTime.Add(DateTime.Now);

        // Check if user has requested more than 5 hints in the last 20 seconds.
        foreach (var item in this.lastPlayTime.Where(x => DateTime.Now - x > TimeSpan.FromSeconds(20)).ToList())
        {
            this.lastPlayTime.Remove(item);
        }

        if (this.lastPlayTime.Count >= 5)
        {
            gameObject.GetComponent<WalkingBehavior>()?.PlayAngryAnimation();
        }

        // Play laughing animation
        var anim = gameObject.GetComponent<Animation>();
        if (!anim.isPlaying)
        {
            anim.Play("Armature|jiggling");
        }
    }
}
