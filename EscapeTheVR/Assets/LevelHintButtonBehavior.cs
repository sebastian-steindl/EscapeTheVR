using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHintButtonBehavior : MonoBehaviour
{
    private int levelID = -1;
    private int nextSoundIndex = 0;

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
        var currentLevel = LevelManager.Instance().getCurrentLevel();
        
        // Reset sound index when the next level is loaded.
        if (currentLevel.levelId != this.levelID)
        {
            this.levelID = currentLevel.levelId;
            this.nextSoundIndex = 0;
        }

        this.PlayNextHint(currentLevel);
    }

    // Cycle through all sounds of the level.
    private void PlayNextHint(Level lvl)
    {
        this.PlayHintSound(lvl.hintFilePaths[this.nextSoundIndex]);

        if (lvl.hintFilePaths.Count > this.nextSoundIndex + 1)
        {
            this.nextSoundIndex++;
        }
        else
        {
            this.nextSoundIndex = 0;
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
