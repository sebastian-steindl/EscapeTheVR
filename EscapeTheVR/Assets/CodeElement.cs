using System;
using System.Xml.Serialization;

public class CodeElement
{
    [XmlAttribute("elementId")]
    public int elementId;

    [XmlAttribute("newLine")]
    public bool newLine = false;

    [XmlText]
    public string codeElement;
}
