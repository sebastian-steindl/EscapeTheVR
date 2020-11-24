﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public class LevelManager
{
    private static LevelManager levelInstance = new LevelManager();
    private List<Level> levels;
    private int currentLevelId;

    private LevelManager() {
        levels = readLevels();
    }

    public static LevelManager Instance() { return levelInstance; }

    public Level getLevelById(int levelID) {
        foreach (Level l in levels)
            if (l.levelId == levelID)
                return l;
        return null;
    }

    public List<Level> getLevels() {
        return levels;
    }

    public Level NextLevel() {
        if ((currentLevelId + 1) == levels.Count)
            return levels[levels.Count-1];
        currentLevelId++;
        return levels[currentLevelId];
    }

    private List<Level> readLevels()
    {
        Regex regex = new Regex("level\\d+.xml");
        List<Level> levels = new List<Level>();
        var dir = new System.IO.DirectoryInfo(Application.dataPath + "/Resources/");
        foreach (System.IO.FileInfo f in dir.GetFiles())
        {
            var name = f.Name;

            //Exclude all files that aren't level files.
            if (name.EndsWith(".meta") || !regex.IsMatch(name))
                continue;

            //Else parse file and add to list
            levels.Add(PuzzleXMLReader.readLevel("/Resources/" + f.Name));
        }
        return levels;
    }
}