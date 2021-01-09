using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementStoneFactory
{
    private static ElementStoneFactory instance = new ElementStoneFactory();

    private ElementStoneFactory()
    {
        
    }

    public static ElementStoneFactory Instance
    {
        get 
        { 
            return instance; 
        }
    }

    public ElementStone createElementStone(string type)
    {
        programmingElement elem = getProgrammingElementByString(type);
        ElementStone es;

        if(elem == programmingElement.elemVar)
            es = new VariableStone();
        else if(elem == programmingElement.elemInterval)
            es = new IntervalStone();
        else
            es = new ElementStone(elem);

        return es;
    }

    public static programmingElement getProgrammingElementByString(string type)
    {
        switch (type)
        {
            case "variable":
                return programmingElement.elemVar;
            case "for":
                return programmingElement.elemFor;
            case "if":
                return programmingElement.elemIf;
            case "while":
                return programmingElement.elemWhile;
            case "==":
                return programmingElement.elemCompare;
            case "&&":
                return programmingElement.elemLogicalAnd;
            case "||":
                return programmingElement.elemLogicalOr;
            case "!":
                return programmingElement.elemNegation;
            case "print()":
                return programmingElement.elemFuncPrint;
            case "text":
                return programmingElement.elemText;
            case "number":
                return programmingElement.elemNumber;
            case "bool":
                return programmingElement.elemBool;
            case "interval":
                return programmingElement.elemInterval;
            case "end":
                return programmingElement.elemEnd;
            case "*=":
                return programmingElement.elemMultiplyEquals;
            case "!=":
                return programmingElement.elemNotEquals;
            default:
                return programmingElement.elemVar;
        }
    }
}
