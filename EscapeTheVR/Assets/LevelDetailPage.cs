using TMPro;
using UnityEngine;

public class LevelDetailPage:Page
{
    private void Update()
    {
        var level = LevelManager.Instance().getLevelById(PlayerPrefs.GetInt(Constants.playerPrefsLevel, 1));
        var textFields = GetComponentsInChildren<TMP_Text>();

        //Fullfill text fields.
        foreach (TMP_Text textfield in textFields)
        {
            if (textfield.name == "LevelName")
                textfield.text = level.name;
            if (textfield.name == "LevelDescription")
                textfield.text = level.descr;
        }
    }
}