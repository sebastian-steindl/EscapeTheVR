using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInventar
{
    private static VRInventar instance = new VRInventar();

    private VRInventar()
    {

    }

    public static VRInventar Instance
    {
        get { return instance; }
    }



}
