using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameBoard 
{
    List<Slot> slots;
    public Puzzle activePuzzle;
    // vr gameboard position and scale
    public Vector3 pos;
    public Vector3 scale;

    private int numberOfSlots;
    private List<Vector3> slotPositions;
    private float slotWidthHeight = 0.25f;

    public GameBoard(Vector3 pos, Vector3 scale)
    {
        this.pos = pos;
        this.scale = scale;
    }
    public GameBoard(Vector3 pos, Vector3 scale, Puzzle puzzle)
    {
        this.pos = pos;
        this.scale = scale;
        activePuzzle = puzzle;
    }

    internal void addSlot(Slot slot)
    {
        slots.Add(slot);
    }

    internal void initSlots()
    {
        numberOfSlots = activePuzzle.getNumberOfSlots();
        generateSlotPositions();

        for (int i = 0; i < numberOfSlots; i++)
        {
            addSlot(new Slot(slotPositions[i], slotWidthHeight, slotWidthHeight));
        }

    }

    internal bool checkIfElementIsPlacedOverASlot(GameObject gameObject)
    {
        // c# 7 tuple syntax, finally you can write code nearly as nice as in python
        (Slot closestSlot, float dist) = getClostestSlotAndDistance(gameObject);

        return closestSlot.isElemCloseEnough(dist);
    }

    internal (Slot, float) getClostestSlotAndDistance(GameObject gameObject)
    {
        int indexOfClosestSlot = 0;
        float closestDist = Mathf.Infinity;
        for (int i = 0; i < numberOfSlots; i++)
        {
            float dist = Vector3.Distance(slots[i].position, gameObject.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                indexOfClosestSlot = i;
            }
        }
        return (slots[indexOfClosestSlot], closestDist);
    }

    internal void generateSlotPositions()
    {
        numberOfSlots = activePuzzle.getNumberOfSlots();

        float padding = 0.1f;
        float marginTopBottom = 0.5f;
        float marginLeftRight = marginTopBottom;

        float availableWidth = scale.z - 2 * marginLeftRight;
        float availableHeight = scale.y - 2 * marginTopBottom;

        float slotX = pos.x;

        int columns = (int)Math.Floor((availableWidth / slotWidthHeight));
        int rows = (int)Math.Ceiling((double)(numberOfSlots / columns));

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float slotY = marginTopBottom + i * (slotWidthHeight + padding);
                float slotZ = marginLeftRight + j * (slotWidthHeight + padding);

                slotPositions.Add(new Vector3(slotX, slotY, slotZ));
            }
        }

    }
}


public class Slot
{
    public Vector3 position;
    private programmingElement? elem; // null if empty
    private float width;
    private float height;
    static float threshold = 15;


    public Slot(Vector3 pos, float width, float height)
    {
        position = pos;
        this.width = width;
        this.height = height;
    }

    public void setElement(programmingElement element)
    {
        elem = element;
    }

    // TODO : does this correctly return null after reset?
    public programmingElement getElement() { return (programmingElement)elem; }

    public void resetElement()
    {
        elem = null;
    }

    public bool isElemCloseEnough(float distance)
    {
        return Math.Abs(distance) < threshold;
    }
}