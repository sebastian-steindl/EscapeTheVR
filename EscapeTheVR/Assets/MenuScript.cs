using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//For menu setup according to https://www.youtube.com/watch?v=__iTtJHZg6k and https://www.youtube.com/watch?v=0otP3ww-auE
public class MenuScript : MonoBehaviour
{

    private Stack<Page> prevPages;
    [SerializeField]
    private Page current;
    // Start is called before the first frame update

    void Start()
    {
        //For loading a new scene... -> https://www.youtube.com/watch?v=-GWjA6dixV4
        //SceneManager.LoadScene("SampleScene");
        prevPages = new Stack<Page>();
        current.Show();
    }

    public void exitMenu() {
        current.Hide();
        this.enabled = false;
    }

    public void showMenu() {
        this.enabled = true;
        current.Show();
    }

    //Go back one settings page
    public void goBack() {
        if (prevPages.Count == 0)
            return;
        current.Hide();
        current = prevPages.Pop();
        current.Show();
    }

    //Set the current settings page
    public void setCurrentPage(Page curr) {
        prevPages.Push(current);
        current.Hide();
        current = curr;
        current.Show();
    }

    public void startGame() {
        SceneManager.LoadScene("SampleScene");
    }

    public void deleteGameElements() {
        // Remove all PuzzleElements
        var puzzleElements = FindObjectsOfType<DragObject>();
        foreach (DragObject d in puzzleElements) {
            Destroy(d.gameObject);
            Destroy(d);
        }

        // Remove GameBoard
        var gameBoard = FindObjectOfType<GameBoardScript>();
        Destroy(gameBoard.gameObject);
        Destroy(gameBoard);
    }

    public void terminateGame() {
        Debug.Log("Application Terminated :) - Just not in debug.");
        Application.Quit();
    }
}
