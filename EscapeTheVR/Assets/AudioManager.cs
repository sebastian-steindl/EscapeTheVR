using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager: MonoBehaviour {
    private static AudioManager instance;

    private AudioSource successSound;


    void Start()
    {
        instance = this;

        var sounds = GetComponents<AudioSource>();

        successSound = sounds[0];
    }

    public static AudioManager Instance
    {
        get { return instance; }
    }

    public void playSuccess()
    {
        successSound.Play();
    }

}
