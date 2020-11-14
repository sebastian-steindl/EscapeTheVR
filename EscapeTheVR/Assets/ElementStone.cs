using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStone
{
    public programmingElement elem; // e.g. for/if 
    public int id; // the position in which it should be placed

    public string descriptionText; // text to describe the stone
    public Color color; // color of the VR stone
    public Color border; // border of the VR stone

    public ElementStone(Color stoneColor, string description=Constants.descriptionDefault)
    {
        descriptionText = description;
        color = stoneColor; 
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
    public ElementStone filledWith;
    public VariableStone(Color stoneColor, string description = Constants.descriptionDefault) : base(stoneColor, description)
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
    elemText, // text for variables (string)
    elemNumber, // number for variables (int)
    elemBool, // bool for variables
    elemInterval, // interval between two ints, e.g. used in for

}
