using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{
    // Start is called before the first frame update

    public GameBoard gameBoard;
    public Puzzle puzzle;

    void Start()
    {
        puzzle = new Puzzle();
        gameBoard = new GameBoard(gameObject.transform.position,gameObject.transform.localScale, puzzle);
        gameBoard.initSlots();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
