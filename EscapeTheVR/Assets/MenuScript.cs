using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//For menu setup according to https://www.youtube.com/watch?v=__iTtJHZg6k and https://www.youtube.com/watch?v=0otP3ww-auE
public class MenuScript : MonoBehaviour
{

    private Stack<Page> prevPages;
    private Page current;
    // Start is called before the first frame update

    void Start()
    {
        //For loading a new scene... -> https://www.youtube.com/watch?v=-GWjA6dixV4
        //SceneManager.LoadScene("SampleScene");
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
        current.Hide();
        current = curr;
        current.Show();
        prevPages.Push(curr);
    }

    public void startGame() {
        Debug.Log(":)");
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
        Application.Quit();
    }
}
