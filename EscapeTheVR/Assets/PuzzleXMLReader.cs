using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class PuzzleXMLReader
{
    public static (Puzzle, Level) readLevel(string filename, bool snapEnabled=false) {
        var serializer = new XmlSerializer(typeof(Level));
        var level = serializer.Deserialize(new FileStream(Application.dataPath+filename, FileMode.Open)) as Level;
        if (level.apiversion == null || !level.apiversion.Equals("10"))
            Debug.Log("Incorrect API-Version detected!");
        Puzzle p = new Puzzle(level.name, level.puzzleProgrammingElements.Count, snapEnabled);
        p.setSolution(getProgrammingElementsFromPuzzleProgrammingElements(level.puzzleProgrammingElements));
        return (p, level);
    }

    internal static List<int> getProgrammingElementsFromPuzzleProgrammingElements(List<PuzzleProgrammingElement> puzzleElements) {
        List<int> list = new List<int>();
        puzzleElements.ForEach(el => {
            if(el.isPartOfSolution)
                list.Add(el.id);
        });
        return list;
    }

}
