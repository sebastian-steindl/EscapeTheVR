using System;
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
        float y = 500;
        float z = 2;
        float scale = 0.005f;
        for (int i = 0; i < levels.Count; i++)
        {
            if (i % 4 == 0)
                y -= 150;
            var curr = createLevelTextFromPrefab(levels[i]);
            curr.transform.localScale = new Vector3(1, 1, 1);
            Vector3 vector3 = new Vector3((-375 + ((i % 4) * 250))*scale, y*scale, z);
            curr.transform.position = vector3;
            //curr.transform.position = new Vector3(curr.transform.position.x / 200, curr.transform.position.y / 194, 2); 

            //curr.transform.position = new Vector3(250 * (i % 4) + 125, y, z);

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
