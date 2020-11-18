using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//For menu setup according to https://www.youtube.com/watch?v=__iTtJHZg6k and https://www.youtube.com/watch?v=0otP3ww-auE
public class Page : MonoBehaviour
{

    private Canvas canvas;
    public bool enableSnapping = true;
    private Toggle enableSnapToggle;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        enableSnapToggle = GetComponentInChildren<Toggle>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide() {
        canvas.enabled= false;
    }

    public void Show() {
        canvas.enabled= true;
    }

    public void setLevel(int levelId) {
        PlayerPrefs.SetInt("levelId", levelId);
    }

    public void ToggleSnap() {
        enableSnapping = !enableSnapping;
        enableSnapToggle.isOn = enableSnapping;
        Debug.Log("ToggleSnap: " + enableSnapping+ "\nPlayerPref.snapEnabled(default): " + PlayerPrefs.GetString("snapEnabled", "default"));
        if(enableSnapping)
        PlayerPrefs.SetString("snapEnabled", "true");
        else
        PlayerPrefs.SetString("snapEnabled", "false");

    }
}
