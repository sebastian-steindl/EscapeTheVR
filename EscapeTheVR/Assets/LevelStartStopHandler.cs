﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
public class LevelStartStopHandler : MonoBehaviour
{
    private static LevelStartStopHandler instance = new LevelStartStopHandler();

    private GameBoardScript gameboard; 
    private LevelStartStopHandler()
    {
        gameboard = FindObjectOfType<GameBoardScript>();
    }

    public static LevelStartStopHandler Instance
    {
        get { return instance; }
    }

    public void StartLevel(int levelId)
    {
        LevelManager.Instance().startLevelTimer();

        Debug.Log("Level" + levelId.ToString() + "started");

        switch (levelId)
        {
            case 1:
                break;
            case 2:
                break;
            case 6:
                // this inverses the game mode, i.e. the code is shown right at the beginngin 
                // TODO --> level hints/beschreibung anpassen
                gameboard.updateCodeText();
                break;
            default:
                Debug.Log("LevelStartStopHandler::StartLevel called but no case was programmed for levelId: " + levelId);
                break;
        }

        // Reset the shelf board on every level
        foreach (var target in FindObjectsOfType<TargetBehavior>())
        {
            target.ControlledObject.SetActive(true);
        }
    }

    public void StopLevel(int levelId)
    {
        LevelManager.Instance().stopLevelTimer();

        switch (levelId)
        {
            case 1:
                Debug.Log("Level 1 stopped");
                break;
            case 2:
                Debug.Log("Level 2 stopped");
                break;
            default:
                Debug.Log("LevelStartStopHandler::StartLevel called but no case was programmed for levelId: " + levelId);
                break;
        }
    }

}
