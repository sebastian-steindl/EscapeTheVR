using System;
using System.Collections;
using System.Collections.Generic;
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
        this.numberOfSlots = activePuzzle.getNumberOfSlots();
        generateSlotPositions();

        for (int i = 0; i < numberOfSlots; i++)
        {
            addSlot(new Slot(slotPositions[i]));
        }

    }

    internal void generateSlotPositions()
    {
        this.numberOfSlots = activePuzzle.getNumberOfSlots();

        float padding = 0.1f;
        float marginTopBottom = 0.5f;
        float marginLeftRight = marginTopBottom;

        float availableWidth = scale.z - 2 * marginLeftRight;
        float availableHeight = scale.y - 2 * marginTopBottom;

        float slotWidthHeight = 0.25f;

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
    Vector3 position;
    private programmingElement? elem; // null if empty

    public Slot(Vector3 pos)
    {
        position = pos;
    }

    public void setElement(programmingElement element)
    {
        elem = element;
    }

    // ToDo: does this correctly return null after reset?
    public programmingElement getElement() { return (programmingElement)elem; }

    public void resetElement()
    {
        elem = null;
    }
}