using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelID;
    public string text;
    private void Start()
    {
        Debug.Log(text);
        GetComponentInChildren<TMP_Text>().text = text;
        GetComponentInChildren<TMP_Text>().fontSize = 32;
    }

    private void Update()
    {
        //GetComponent<TextMeshPro>().text = text;
    }

    public void onTriggerDown() {
        PlayerPrefs.SetInt(Constants.playerPrefsLevel, levelID);
        FindObjectOfType<MenuScript>().setCurrentPage(FindObjectOfType<LevelDetailPage>());
    }

}
