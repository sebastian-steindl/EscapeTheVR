using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStone
{
    public programmingElement elem; // e.g. for/if 
    public int positionInProgram; // the position in which it should be placed

    public string descriptionText; // text to describe the stone
    public Color color; // color of the VR stone
    public Color border; // border of the VR stone

    public ElementStone()
    {
        descriptionText = "No description";
        color = new Color(209, 186, 111); // should be changed to nice looking default later
    }

    public void greenBorder()
    {
        border = Color.green;
    }
    public void redBorder()
    {
        border = Color.red;
    }
}

public class VariableStone : ElementStone
{
    public VariableStone()
    {
        elem = programmingElement.elemVar;
    }
}
public enum programmingElement
{
    // feel free to add stuff
    // use prefix "elem" bc if/while etc are forbidden
    elemVar, // variable
    elemFor,
    elemIf,
    elemWhile,
    elemCompare, // ==
    elemNegation, // !
    elemLogicalAnd, // &&
    elemLogicalOr, // ||
    elemFuncPrint, // print()
}
