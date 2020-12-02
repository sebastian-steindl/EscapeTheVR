using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    private static LevelManager levelInstance = new LevelManager();
    private List<Level> levels;
    private int currentLevelId = 1;
    private Stopwatch stopwatch;
    private bool done = false;

    private LevelManager() {
        levels = readLevels();
        stopwatch = new Stopwatch();
    }

    public static LevelManager Instance() { return levelInstance; }

    public Level getCurrentLevel()
    {
        return getLevel(currentLevelId ==0 ? 0 : currentLevelId - 1);//Since there is currently no level with the id 0, there needs to be this offset by one. 
    }

    public Level getLastLevel() {
        return getLevel(currentLevelId < 2 ? 0 : currentLevelId - 2);
    }

    private Level getLevel(int id) {
        return levels[id];
    }

    public Level getLevelById(int levelID) {
        //Go through all levels and check the id, if correct, return if nothing is found, return null;
        foreach (Level l in levels)
            if (l.levelId == levelID)
                return l;
        return null;
    }

    public List<Level> getLevels() {
        return levels;
    }

    public void NextLevel() {
        //If you have finished all levels, you will be stuck at the current one. --> TODO: better version.
        if ((currentLevelId + 1) <= levels.Count)
        {
            //Increment current level.
            currentLevelId++;
            PlayerPrefs.SetInt(Constants.playerPrefsLevel, currentLevelId);
            UnityEngine.Debug.Log("SET LevelID: " +currentLevelId+"\t PlayerPrefs: "+ PlayerPrefs.GetInt(Constants.playerPrefsLevel, 1));
        }
        else
            done = true;

        SceneManager.LoadScene("ChangeScene");
    }

    public void startLevelTimer() {
        stopwatch.Reset();
        stopwatch.Start();
    }

    public void stopLevelTimer() {
        stopwatch.Stop();
    }

    public string getLevelTime() {
        var time = stopwatch.Elapsed;
        var sb = new StringBuilder();
        //Add Hours if applicable
        if (time.Hours > 0)
            sb.Append(time.Hours).Append("H ");
        //Add leading 0 if minutes is less than 10
        if (time.Minutes >0 && time.Minutes < 10)
            sb.Append("0");
        if (time.Minutes > 0)
            sb.Append(time.Minutes).Append("Min ");
        sb.Append(time.Seconds).Append("s");
        return sb.ToString();
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

    public void refreshLevels() {
        this.levels = readLevels();
    }


    public bool areYouDone() {
        return done;
    }

    public String getCurrentLevelElements() {
        Dictionary<string, int> data = new Dictionary<string, int>();
        //Count all elements 
        foreach (PuzzleProgrammingElement p in getCurrentLevel().puzzleProgrammingElements) {
            if (data.ContainsKey(p.type))
                data[p.type]++;
            else
                data.Add(p.type, 1);
        }

        //Create final String.
        StringBuilder sb = new StringBuilder();
        foreach (string s in data.Keys)
            sb.Append(data[s]).Append("x ").Append(s).Append("\n");

        return sb.ToString();
    }
}