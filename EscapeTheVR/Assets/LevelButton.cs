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
    }

    private void Update()
    {
        //GetComponent<TextMeshPro>().text = text;
    }

    public void onTriggerDown() {
        Debug.Log("I will change the page. Current ID is: "+levelID);
        PlayerPrefs.SetInt("level", levelID);
        FindObjectOfType<MenuScript>().setCurrentPage(FindObjectOfType<LevelDetailPage>());
    }

}
