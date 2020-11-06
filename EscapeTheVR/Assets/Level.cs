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

    [XmlArray("program")]
    [XmlArrayItem("element")]
    public List<PuzzleProgrammingElement> puzzleProgrammingElements = new List<PuzzleProgrammingElement>();
}