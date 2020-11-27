using System;
using System.Xml.Serialization;

public class PuzzleProgrammingElement
{
    public int id;

    public string type;

    public float positionX;

    public float positionY;

    public float positionZ;

    public string text;

    public string hintPath;

    [XmlAttribute("isPartOfSolution")]
    public bool isPartOfSolution = false;

    [XmlAttribute("valueOfId")]
    public int valueOfId;
}