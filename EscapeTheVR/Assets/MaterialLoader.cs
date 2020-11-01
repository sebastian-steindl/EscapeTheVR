using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLoader
{
    private static MaterialLoader instance = new MaterialLoader();

    private MaterialLoader()
    {

    }

    public static MaterialLoader Instance
    {
        get { return instance; }
    }

    /*
     *param programmingElementName
    * Returns a material named "Assets/Resources/[name]" to the object. 
    */
    public Material getMaterial(string programmingElementName)
    {
        //https://answers.unity.com/questions/13356/how-can-i-assign-materials-using-c-code.html

        string path = getMaterialFileForProgrammingType(programmingElementName);
        return Resources.Load(path, typeof(Material)) as Material;
    }

    private string getMaterialFileForProgrammingType(string programmingElementName)
    {
        switch (programmingElementName)
        {
            case "variable":
                return "RedMaterial";
            //case "for":
            //    return programmingElement.elemFor;
            //case "if":
            //    return programmingElement.elemIf;
            //case "while":
            //    return programmingElement.elemWhile;
            //case "==":
            //    return programmingElement.elemCompare;
            //case "&&":
            //    return programmingElement.elemLogicalAnd;
            //case "||":
            //    return programmingElement.elemLogicalOr;
            //case "!":
            //    return programmingElement.elemNegation;
            case "print()":
                return "GreenMaterial";
            default:
                return "/";
        }
    }
}
