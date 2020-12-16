﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LevelPage : Page
{

    private void Start()
    {
        List<Level> levels = LevelManager.Instance().getLevels();
        //Currently only optimized for 4 levels in a row, 8 levels total.
        float y = 470.0f;
        for (int i = 0; i < levels.Count; i++)
        {
            if(i%4==0)
                y-=130;
            var curr = createLevelTextFromPrefab(levels[i]);
            curr.transform.position = new Vector3((250 * (i % 4) + 125), y, 0.0f);
        }
    }

    public GameObject createLevelTextFromPrefab(Level level)
    {
        GameObject test = Resources.Load("TextPrefab", typeof(GameObject)) as GameObject;
        GameObject textobj = Instantiate(test);
        textobj.GetComponent<LevelButton>().levelID = level.levelId;
        textobj.GetComponent<LevelButton>().text = level.name;
        textobj.transform.parent = this.transform;
        return textobj;
    }
}
