using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;

public class GameBoard
{
    private int numberOfSlots;
    private List<Vector3> slotPositions;
    private float slotWidthHeight = 0.25f;
    private List<Slot> slots;
    public Puzzle activePuzzle;

    // vr gameboard position and scale
    public Vector3 pos;
    public Vector3 scale;

    public GameBoard(Vector3 pos, Vector3 scale, Puzzle puzzle = null)
    {
        this.pos = pos;
        this.scale = scale;
        activePuzzle = puzzle;

        slots = new List<Slot>();
        slotPositions = new List<Vector3>();
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
            addSlot(new Slot(slotPositions.ElementAt(i), slotWidthHeight, slotWidthHeight));
        }
    }

    internal (bool, Slot) checkIfElementIsPlacedOverASlot(GameObject gameObject)
    {
        // c# 7 tuple syntax, finally you can write code nearly as nice as in python
        (Slot closestSlot, float dist) = getClostestSlotAndDistance(gameObject);

        return (closestSlot.isElemCloseEnough(dist), closestSlot);
    }

    internal (Slot, float) getClostestSlotAndDistance(GameObject gameObject)
    {
        if (!gameObject) return (null, Mathf.Infinity);

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

        float padding = 1.5f * slotWidthHeight;
        float marginTopBottom = 0.5f;
        float marginLeftRight = marginTopBottom;

        float availableWidth = scale.z - 2 * marginLeftRight;
        float availableHeight = scale.y - 2 * marginTopBottom;
        float xOffset = 0.5f;
        float slotX = pos.x + xOffset;

        int columns = (int)Math.Floor(availableWidth / slotWidthHeight);

        if (columns > numberOfSlots) columns = numberOfSlots;

        int rows = (int)Math.Ceiling((double)(numberOfSlots / columns));
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float slotY = pos.y + marginTopBottom + i * (slotWidthHeight + padding);
                float slotZ = pos.z + marginLeftRight + j * (slotWidthHeight + padding);
                Debug.Log("Adding position: x, y, z" + slotX + ", " + slotY + ", " + slotZ);
                slotPositions.Add(new Vector3(slotX, slotY, slotZ));
            }
        }
        Debug.Log("after generation: number" + slotPositions.Count);
    }

    public List<Slot> getSlots() { return slots; }
    public Puzzle getPuzzle() {return activePuzzle;}
}