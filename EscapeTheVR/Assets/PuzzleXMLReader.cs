using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class PuzzleXMLReader
{
    public static Puzzle readLevel(string filename, bool snapEnabled=false) {
        var serializer = new XmlSerializer(typeof(Level));
        var level = serializer.Deserialize(new FileStream(Application.dataPath+filename, FileMode.Open)) as Level;
        if (level.apiversion == null || !level.apiversion.Equals("10"))
            Debug.Log("Incorrect API-Version detected!");
        Puzzle p = new Puzzle(level.name, level.puzzleProgrammingElements.Count, snapEnabled);
        p.setSolution(getProgrammingElementsFromPuzzleProgrammingElements(level.puzzleProgrammingElements));
        //TODO: Generate the CodeElements as DragObjects at the given coords.
        level.puzzleProgrammingElements.ForEach(el => createElement(el));
        return p;
    }

    /*
     If element could not be matched, the default datatype will be var.
     public static programmingElement getProgrammingElementFromString(string element) {
    programmingElement el = programmingElement.elemVar;
        switch (element) {
            case "elemCompare":
                el =  programmingElement.elemCompare;
                break;
            case "elemFor":
                el = programmingElement.elemFor;
                break;
            case "elemFuncPrint":
                el = programmingElement.elemFuncPrint;
                break;
            case "elemIf":
                el = programmingElement.elemIf;
                break;
            case "elemLogicalAnd":
                el = programmingElement.elemLogicalAnd;
                break;
            case "elemLogicalOr":
                el = programmingElement.elemLogicalOr;
                break;
            case "elemNegation":
                el = programmingElement.elemNegation;
                break;
            case "elemVar":
                el = programmingElement.elemVar;
                break;
            case "elemWhile":
                el = programmingElement.elemWhile;
                break;
        }
        return el;
    }
    */

    internal static List<programmingElement> getProgrammingElementsFromPuzzleProgrammingElements(List<PuzzleProgrammingElement> puzzleElements) {
        List<programmingElement> list = new List<programmingElement>();
        puzzleElements.ForEach(el => list.Add(ElementStoneFactory.getProgrammingElementByString(el.type)));
        return list;
    }


    internal static DragObject createElement(PuzzleProgrammingElement puzzleElement) {
        GameObject.CreatePrimitive(PrimitiveType.Quad);
        var data = new GameObject("Hallo", typeof(DragObject));
        data.AddComponent<DragObject>();
        data.GetComponent<DragObject>();
        DragObject drag = new DragObject(puzzleElement.type);
        drag.transform.position = new Vector3(puzzleElement.positionX, puzzleElement.positionY, puzzleElement.positionZ);
        return drag;
        //return null;
    }

}
