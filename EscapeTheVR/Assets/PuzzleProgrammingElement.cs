using System;
using System.Xml.Serialization;

public class PuzzleProgrammingElement
{
    public string type;

    public float positionX;

    public float positionY;

    public float positionZ;

    public string text;

    [XmlAttribute]
    public bool isPartOfSolution = false;
}