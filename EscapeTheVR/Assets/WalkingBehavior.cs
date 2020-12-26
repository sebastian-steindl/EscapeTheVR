using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WalkingBehavior : MonoBehaviour
{
    enum State
    {
        WalkLeft,
        StayLeft,
        TurnRight,
        WalkRight,
        StayRight,
        TurnLeft
    }

    State currentState;
    DateTime waitingStarted;

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.WalkLeft;
    }

    // Update is called once per frame
    void Update()
    {
        var anim = gameObject.GetComponent<Animation>();

        switch (currentState)
        {
            case State.WalkLeft:
                transform.position += new Vector3(0, 0, 0.5f * Time.deltaTime);

                if (!anim.isPlaying)
                {
                    anim.Play("Armature|walking");
                }

                if (transform.position.z > -2)
                {
                    this.waitingStarted = DateTime.Now;
                    this.currentState = State.StayLeft;
                    anim.Stop();
                }
                return;
            case State.WalkRight:
                transform.position += new Vector3(0, 0, -0.5f * Time.deltaTime);

                if (!anim.isPlaying)
                {
                    anim.Play("Armature|walking");
                }

                if (transform.position.z < -7)
                {
                    this.waitingStarted = DateTime.Now;
                    this.currentState = State.StayRight;
                    anim.Stop();
                }
                return;
            case State.TurnLeft:
                transform.Rotate(0, -50 * Time.deltaTime, 0);

                if (!anim.isPlaying)
                {
                    anim.Play("Armature|walking");
                }

                if (transform.rotation.eulerAngles.y < 90)
                {
                    this.currentState = State.WalkLeft;
                    anim.Play("Armature|walking");
                }
                return;
            case State.TurnRight:
                transform.Rotate(0, 50 * Time.deltaTime, 0);

                if (!anim.isPlaying)
                {
                    anim.Play("Armature|walking");
                }

                if (transform.rotation.eulerAngles.y > 270)
                {
                    this.currentState = State.WalkRight;
                }

                return;
            case State.StayLeft:

                if ((DateTime.Now - this.waitingStarted).TotalSeconds > 30)
                {
                    this.currentState = State.TurnRight;
                    anim.Play("Armature|walking");
                }
                return;
            case State.StayRight:

                if ((DateTime.Now - this.waitingStarted).TotalSeconds > 30)
                {
                    this.currentState = State.TurnLeft;
                    anim.Play("Armature|walking");
                }
                return;
        }
    }

    private void OnMouseDown()
    {
        
    }
}
