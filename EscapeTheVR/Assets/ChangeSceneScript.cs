using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var levelMan = LevelManager.Instance();
        var textFields = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text t in textFields)
        {
            if (t.name == "LevelText")
            {
                t.text = levelMan.getLastLevel().name;
                continue;
            }
            if (t.name == "TimeText")
            {
                t.text = levelMan.getLevelTime();
                continue;
            }
        }

        //Switch buttons, if you have finished all levels.
        var data = FindObjectsOfType<Button>();
        if (levelMan.areYouDone())
        {
            foreach (Button b in data)
            {
                if (b.name == "ButtonNextChallenge")
                    b.gameObject.SetActive(false);
                if (b.name == "ButtonMenu")
                    b.gameObject.SetActive(true);
            }
        }
        else
            foreach (Button b in data)
            {
                if (b.name == "ButtonNextChallenge")
                    b.gameObject.SetActive(true);
                if (b.name == "ButtonMenu")
                    b.gameObject.SetActive(false);
            }
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void startLevel()
    {
        LevelManager.Instance().NextLevel();
        SceneManager.LoadScene("GameScene");
    }

    public void goToMenu() {
        SceneManager.LoadScene("Menu");
    }
}
