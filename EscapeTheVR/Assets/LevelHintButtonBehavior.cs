using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelHintButtonBehavior : MonoBehaviour
{
    private int levelID = -1;
    private int nextSoundIndex = 0;
    private Collider lastCollider = null;
    private List<DateTime> lastPlayTime = new List<DateTime>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        var currentLevel = LevelManager.Instance().getCurrentLevel();
        
        // Reset sound index when the next level is loaded.
        if (currentLevel.levelId != this.levelID)
        {
            this.levelID = currentLevel.levelId;
            this.nextSoundIndex = 0;
        }

        this.PlayNextHint(currentLevel);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Debug.Log(other.name);
        if (other.name == "Sphere (2)")
        {
            OnMouseDown();
            lastCollider = other;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(lastCollider == other)
        {
            lastCollider = null;
        }
    }

    // Cycle through all sounds of the level.
    private void PlayNextHint(Level lvl)
    {
        this.lastPlayTime.Add(DateTime.Now);

        // Check if user has requested more than 5 hints in the last 20 seconds.
        foreach (var item in this.lastPlayTime.Where(x => DateTime.Now - x > TimeSpan.FromSeconds(20)).ToList())
        {
            this.lastPlayTime.Remove(item);
        }

        if (this.lastPlayTime.Count >= 5)
        {
            FindObjectOfType<WalkingBehavior>()?.PlayAngryAnimation();
        }
        else
        {
            this.PlayHintSound(lvl.hintFilePaths[this.nextSoundIndex]);
            this.nextSoundIndex = ++this.nextSoundIndex % lvl.hintFilePaths.Count;
        }
    }

    public void PlayHintSound(string hintSoundPath)
    {
        var gameObject = GameObject.FindGameObjectWithTag("HintAudioSource");
        var audioSource = gameObject.GetComponent<AudioSource>();
        var clip = Resources.Load<AudioClip>(hintSoundPath);
        audioSource.clip = clip;
        audioSource.Play();
    }
}
