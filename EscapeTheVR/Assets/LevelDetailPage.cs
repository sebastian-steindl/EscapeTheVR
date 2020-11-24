using TMPro;
using UnityEngine;

public class LevelDetailPage:Page
{
    public void Start()
    {

        var level = LevelManager.Instance().getLevelById(PlayerPrefs.GetInt("level", 1));
        var textFields = GetComponentsInChildren<TMP_Text>();

        //Fullfill text fields.
        foreach (TMP_Text textfield in textFields)
        {
            Debug.Log(textfield.name);
            if (textfield.name == "LevelName")
                textfield.text = level.name;
            if (textfield.name == "LevelDescription")
                textfield.text = level.descr;
        }

    }
}