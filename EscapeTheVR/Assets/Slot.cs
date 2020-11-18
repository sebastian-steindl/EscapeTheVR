﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class representing the slot to fit a element.
/// </summary>
public class Slot
{
    public Vector3 position;
    private ElementStone elem; // null if empty
    private float width;
    private float height;
    static float threshold = 0.3f;

    public Slot(Vector3 pos, float width, float height)
    {
        position = pos;
        this.width = width;
        this.height = height;
    }

    public void setElement(ElementStone element)
    {
        elem = element;
    }

    public ElementStone getElement()
    {
        return elem;
    }

    public void resetElement()
    {
        elem = null;
    }

    public bool isEmpty()
    {
        return elem == null;
    }

    public bool isElemCloseEnough(float distance)
    {
        return Math.Abs(distance) < threshold;
    }
}
