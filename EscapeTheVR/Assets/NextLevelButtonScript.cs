using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButtonScript : MonoBehaviour
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
        Debug.Log("We are going to change the scene.");
        if(FindObjectOfType<GameBoardScript>().puzzle.isSolved)
            SceneManager.LoadScene("ChangeScene");
    }

    //I'm not sure if this will be necessary for the button to work in VR...
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnterCalled!");
        OnMouseDown();
    }
}
