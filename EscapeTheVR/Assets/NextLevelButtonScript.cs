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
        if (FindObjectOfType<GameBoardScript>().puzzle?.isSolved == true)
        {
            SceneManager.LoadScene("ChangeScene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnterCalled!");
        OnMouseDown();
    }
}
