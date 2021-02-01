using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This class is only for creating the GameBoard element. The GameBoard object will create everything else in the scene.
public class GameSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Launching GameScene");

        //Create GameBoard object. 
        var gameBoard = Instantiate(Resources.Load("gameBoard", typeof(GameObject)) as GameObject);
        gameBoard.transform.position = new Vector3(4.9f, 3f, -9.16f); //TODO: either pack coords into xml or into constants.
        //Done. There is nothing else to instanciate here, since everything else that needs to be created will be created by the GameBoard.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
