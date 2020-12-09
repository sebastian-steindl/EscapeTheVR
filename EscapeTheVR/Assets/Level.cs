using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("level")]
public class Level
{        
    public string apiversion;

    public int levelId;
   
    public string name;

    public string descr;

    public string output;

    public bool startFromCode = false; //Internally also known as "inverse mode"

    [XmlArray("program")]
    [XmlArrayItem("element")]
    public List<PuzzleProgrammingElement> puzzleProgrammingElements = new List<PuzzleProgrammingElement>();

    [XmlArray("code")]
    [XmlArrayItem("codeElement")]
    public List<CodeElement> codeElements = new List<CodeElement>();

    [XmlArray("hintFiles")]
    [XmlArrayItem("hintFile")]
    public List<string> hintFilePaths = new List<string>();
}