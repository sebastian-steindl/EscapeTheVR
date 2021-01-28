using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioQueue : MonoBehaviour
{
    public AudioSource AudioSource;

    private List<string> queue = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.AudioSource != null && this.queue.Count > 0 && !this.AudioSource.isPlaying)
        {
            // Play next audio clip
            this.AudioSource.clip = Resources.Load<AudioClip>(this.queue[0]);
            this.AudioSource.Play();
            this.queue.RemoveAt(0);
        }
    }

    public void Add(string audiofile)
    {
        this.queue.Add(audiofile);
    }
}
