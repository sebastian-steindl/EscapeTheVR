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

    private State currentState;
    private DateTime waitingStarted;
    private Animation anim;
    private AudioSource audioSource;
    private string lastPlayingAnimation = "";

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.WalkLeft;
        this.anim = gameObject.GetComponent<Animation>();
        this.audioSource = gameObject.GetComponent<AudioQueue>().AudioSource;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.WalkLeft:
                transform.position += new Vector3(0, 0, 0.5f * Time.deltaTime);

                if (!anim.isPlaying)
                {
                    anim.Play("Armature|walking");
                    this.lastPlayingAnimation = "Armature|walking";
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
                    this.lastPlayingAnimation = "Armature|walking";
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
                    this.lastPlayingAnimation = "Armature|walking";
                }

                if (transform.rotation.eulerAngles.y < 90)
                {
                    this.currentState = State.WalkLeft;
                    anim.Play("Armature|walking");
                    this.lastPlayingAnimation = "Armature|walking";
                }
                return;
            case State.TurnRight:
                transform.Rotate(0, 50 * Time.deltaTime, 0);

                if (!anim.isPlaying)
                {
                    anim.Play("Armature|walking");
                    this.lastPlayingAnimation = "Armature|walking";
                }

                if (transform.rotation.eulerAngles.y > 270)
                {
                    this.currentState = State.WalkRight;
                }

                return;
            case State.StayLeft:

                if (audioSource.isPlaying)
                {
                    // Teacher is speaking, so the speaking animation should be played. Override idle animation
                    if (!anim.isPlaying || this.lastPlayingAnimation == "Armature|idle")
                    {
                        anim.Play("Armature|speaking");
                        this.lastPlayingAnimation = "Armature|speaking";
                    }
                }
                else
                {
                    if (!anim.isPlaying)
                    {
                        anim.Play("Armature|idle");
                        this.lastPlayingAnimation = "Armature|idle";
                    }
                }

                if ((DateTime.Now - this.waitingStarted).TotalSeconds > 30)
                {
                    this.currentState = State.TurnRight;
                    anim.Play("Armature|walking");
                    this.lastPlayingAnimation = "Armature|walking";
                }
                return;
            case State.StayRight:

                if (audioSource.isPlaying)
                {
                    // Teacher is speaking, so the speaking animation should be played. Override idle animation
                    if (!anim.isPlaying || this.lastPlayingAnimation == "Armature|idle")
                    {
                        anim.Play("Armature|speaking");
                        this.lastPlayingAnimation = "Armature|speaking";
                    }
                }
                else
                {
                    if (!anim.isPlaying)
                    {
                        anim.Play("Armature|idle");
                        this.lastPlayingAnimation = "Armature|idle";
                    }
                }

                if ((DateTime.Now - this.waitingStarted).TotalSeconds > 30)
                {
                    this.currentState = State.TurnLeft;
                    anim.Play("Armature|walking");
                    this.lastPlayingAnimation = "Armature|walking";
                }
                return;
        }
    }

    public void PlayAngryAnimation()
    {
        if (currentState == State.StayLeft || currentState == State.StayRight)
        {
            anim.Play("Armature|readhead");
            this.lastPlayingAnimation = "Armature|readhead";
        }

        // Stop currently playing audio and play angry voice sound
        var audioQueue = gameObject.GetComponent<AudioQueue>();
        audioQueue.Flush();
        audioQueue.Add("Audio/unterlassen_sie_das");
    }

    private void OnMouseDown()
    {
        
    }
}
