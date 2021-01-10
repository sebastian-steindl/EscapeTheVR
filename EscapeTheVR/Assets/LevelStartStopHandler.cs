using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelStartStopHandler : MonoBehaviour
{
    private static LevelStartStopHandler instance = new LevelStartStopHandler();

    private GameBoardScript gameboard;

    private readonly List<string> descriptionSounds = new List<string>()
    {
        "Audio/Intro/begruessung",
        "Audio/Intro/erklaerung_1",
        "Audio/Intro/erklaerung_2",
        "Audio/Intro/erklaerung_3",
        "Audio/Intro/erklaerung_4",
        "Audio/Intro/erklaerung_5",
        "Audio/Intro/erklaerung_6",
        "Audio/Intro/erklaerung_7",
        "Audio/Intro/hilfe",
        "Audio/Intro/reset",
        "Audio/Intro/loesung",
        "Audio/Intro/loesung_2",
        "Audio/Intro/viel_spass_4"
    };

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
            case 0:
                var audioQueue = FindObjectOfType<AudioQueue>();
                foreach (string audio in this.descriptionSounds)
                {
                    audioQueue.Add(audio);
                }
                
                break;
            case 1:
                break;
            case 2:
                break;

            case 3:
                var positionChange = new Vector3(0, 2f, 0);
                gameboard.transform.position += positionChange;
                gameboard.updateSlotPositions(positionChange);
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
            case 3:
                // TODO: This is probably not needed, since they are created again with the beginning of the next level
                var positionChange = new Vector3(0, -2f, 0);
                gameboard.transform.position += positionChange;
                gameboard.updateSlotPositions(positionChange);
                break;
            default:
                Debug.Log("LevelStartStopHandler::StartLevel called but no case was programmed for levelId: " + levelId);
                break;
        }
    }

}
