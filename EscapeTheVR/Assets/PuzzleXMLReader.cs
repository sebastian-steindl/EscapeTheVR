using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class PuzzleXMLReader
{
    public static (Puzzle, Level) readLevel(string filename, bool snapEnabled = false)
    {
        var serializer = new XmlSerializer(typeof(Level));
        var level = serializer.Deserialize(new FileStream(Application.dataPath + filename, FileMode.Open)) as Level;
        if (level.apiversion == null || !level.apiversion.Equals("10"))
            Debug.Log("Incorrect API-Version detected!");
        Puzzle p = new Puzzle(level.name, level.puzzleProgrammingElements.Count, snapEnabled);
        p.setSolution(getSolutionFromXMLPuzzle(level.puzzleProgrammingElements));
        return (p, level);
    }

    internal static List<ElementStone> getSolutionFromXMLPuzzle(List<PuzzleProgrammingElement> puzzleElements)
    {
        List<ElementStone> list = new List<ElementStone>();
        puzzleElements.ForEach(el =>
        {
            if (el.isPartOfSolution)
            {
                switch (ElementStoneFactory.getProgrammingElementByString(el.type))
                {
                    // we assume that the content ALWAYS comes after the container
                    case programmingElement.elemVar:
                        VariableStone variableStone = (VariableStone)ElementStoneFactory.Instance.createElementStone(el.type);
                        variableStone.id = el.id;
                        list.Add(variableStone);
                        break;
                    case programmingElement.elemInterval:
                        IntervalStone intervalStone = (IntervalStone)ElementStoneFactory.Instance.createElementStone(el.type);
                        intervalStone.id = el.id;
                        list.Add(intervalStone);
                        break;
                    case programmingElement.elemBool:
                        ElementStone filledWithStone = ElementStoneFactory.Instance.createElementStone(el.type);
                        filledWithStone.descriptionText = el.text;
                        filledWithStone.id = el.id;
                        (list.Find(pElement => pElement.id == el.valueOfId) as VariableStone).filledWith = filledWithStone;
                        break;
                    case programmingElement.elemText:
                        ElementStone filledStone = ElementStoneFactory.Instance.createElementStone(el.type);
                        filledStone.descriptionText = el.text;
                        filledStone.id = el.id;
                        (list.Find(pElement => pElement.id == el.valueOfId) as VariableStone).filledWith = filledStone;
                        break;
                    case programmingElement.elemNumber:
                        ElementStone fillStone = ElementStoneFactory.Instance.createElementStone(el.type);
                        fillStone.descriptionText = el.text;
                        fillStone.id = el.id;
                        var correspondingElement = list.Find(pElement => pElement.id == el.valueOfId);
                        if (correspondingElement is VariableStone)
                            (list.Find(pElement => pElement.id == el.valueOfId) as VariableStone).filledWith = fillStone;
                        else if (correspondingElement is IntervalStone)
                        {
                            IntervalStone corrElem = correspondingElement as IntervalStone;
                            if (corrElem.from == null) // the from has not been set yet, so the current element is the from element
                                (list.Find(pElement => pElement.id == el.valueOfId) as IntervalStone).from = fillStone;
                            else (list.Find(pElement => pElement.id == el.valueOfId) as IntervalStone).to = fillStone;
                        }
                        else Debug.LogError("Something wrent wrong, don't even try to fix it :)");
                        break;
                    default:
                        ElementStone es = ElementStoneFactory.Instance.createElementStone(el.type);
                        es.id = el.id;
                        list.Add(es);
                        break;
                }
            }
        });
        return list;
    }

}
