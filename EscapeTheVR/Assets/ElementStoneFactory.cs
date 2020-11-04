using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementStoneFactory
{
    private static ElementStoneFactory instance = new ElementStoneFactory();

    private ElementStoneFactory()
    {
        
    }

    public static ElementStoneFactory Instance
    {
        get { return instance; }
    }
     

    public ElementStone createElementStone(string type)
    {
        programmingElement elem = getProgrammingElementByString(type);
        Color color = getStoneColor(elem);
        string description = getDescription(elem);

        ElementStone es = new ElementStone(color, description);
        es.elem = elem;

        return es;

    }

    private string getDescription(programmingElement elem)
    {
        // todo fill switch case
        switch (elem)
        {
            case programmingElement.elemVar:
                break;
            case programmingElement.elemFor:
                break;
            case programmingElement.elemIf:
                break;
            case programmingElement.elemWhile:
                break;
            case programmingElement.elemCompare:
                break;
            case programmingElement.elemNegation:
                break;
            case programmingElement.elemLogicalAnd:
                break;
            case programmingElement.elemLogicalOr:
                break;
            case programmingElement.elemFuncPrint:
                return Constants.descriptionPrintStone;

            case programmingElement.elemText:
                break;
            case programmingElement.elemNumber:
                break;
            case programmingElement.elemBool:
                break;
            case programmingElement.elemInterval:
                break;
            default:
                return Constants.descriptionDefault;
        }

        return Constants.descriptionDefault;
    }

    private Color getStoneColor(programmingElement elem)
    {
        // todo fill switch case
        switch (elem)
        {
            case programmingElement.elemVar:
                return Constants.colorVar;
            case programmingElement.elemFor:
                break;
            case programmingElement.elemIf:
                break;
            case programmingElement.elemWhile:
                break;
            case programmingElement.elemCompare:
                break;
            case programmingElement.elemNegation:
                break;
            case programmingElement.elemLogicalAnd:
                break;
            case programmingElement.elemLogicalOr:
                break;
            case programmingElement.elemFuncPrint:
                return Constants.colorPrint;
            case programmingElement.elemText:
                break;
            case programmingElement.elemNumber:
                break;
            case programmingElement.elemBool:
                break;
            case programmingElement.elemInterval:
                break;
            default:
                break;
        }

        return Constants.colorVar;
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
            default:
                return programmingElement.elemVar;
        }
    }
}
