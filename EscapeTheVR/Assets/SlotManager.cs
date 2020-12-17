using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;

public class SlotManager
{
    private int numberOfSlots;
    private List<Vector3> slotPositions;
    private float slotWidthHeight = 0.25f;
    public List<Slot> slots { get; }

    // vr gameboard position and scale
    public Vector3 pos;
    public Vector3 scale;

    public float marginTopBottom = 0.35f;
    public float marginLeftRight = 0.25f;

    public SlotManager(Vector3 pos, Vector3 scale, int slotNum = 1)
    {
        this.pos = pos;
        this.scale = scale;

        slots = new List<Slot>();
        slotPositions = new List<Vector3>();
        numberOfSlots = slotNum;
    }

    internal void addSlot(Slot slot)
    {
        slots.Add(slot);
    }

    internal void initSlots()
    {
        slots.Clear(); //empty list
        generateSlotPositions();
        for (int i = 0; i < numberOfSlots; i++)
        {
            addSlot(new Slot(slotPositions.ElementAt(i), slotWidthHeight, slotWidthHeight));
        }
    }

    internal void unlockAllDragObjects()
    {
        slots.ForEach(s =>
        {
            s.GetDragObject().isLocked = false;
        });
    }

    internal (bool, Slot) checkIfElementIsPlacedOverASlot(DragObject gameObject)
    {
        // c# 7 tuple syntax, finally you can write code nearly as nice as in python
        (Slot closestSlot, float dist) = getClostestSlotAndDistance(gameObject);

        return (closestSlot.isElemCloseEnough(dist), closestSlot);
    }

    internal (bool isCloseEnough, Slot closestSlot) handlesElementToSlotRelation(DragObject selectedGameObj)
    {
        var selectedElemnt = selectedGameObj.element;
        //Get Distance
        (bool isCloseEnough, Slot closestSlot) = checkIfElementIsPlacedOverASlot(selectedGameObj);
        if (isCloseEnough)
        {
            //If the latest slot is not equal to the current one and is populated by the current element, reset that slot.
            var selObjIndex = Find(selectedGameObj);
            if (selObjIndex >= 0 && selObjIndex != slots.IndexOf(closestSlot))
            {
                Debug.Log("Reseted latest Slot: " + selObjIndex);
                slots[selObjIndex].resetElement();
            }

            //Debug.Log("***Close enough***");
            closestSlot.setElement(selectedGameObj);
        }
        else 
        {
            // if slot isn't close enough but the current element is still set in the slot, remove it.
            var selectedObjIndex = Find(selectedGameObj);
            if (Find(selectedGameObj) > -1)
            {
                Debug.Log("Would have Reset Slot No " + Find(selectedGameObj));
                slots[Find(selectedGameObj)].resetElement();
            }
        }
        return (isCloseEnough, closestSlot);
    }

    internal float getClostestDistance(DragObject gameObject)
    {
        if (!gameObject) return Mathf.Infinity;

        float closestDist = Mathf.Infinity;
        for (int i = 0; i < numberOfSlots; i++)
        {
            float dist = Vector3.Distance(slots[i].position, gameObject.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
            }
        }
        return closestDist;
    }

    internal (Slot, float) getClostestSlotAndDistance(DragObject gameObject)
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
        float padding = 1.5f * slotWidthHeight;

        float availableWidth = scale.z - 2 * marginLeftRight;
        float availableHeight = scale.y - 2 * marginTopBottom;
        float zOffset = 1.2f;
        float slotZ = pos.z + zOffset;

        int columns = (int)Math.Floor(availableWidth / slotWidthHeight);

        if (columns > numberOfSlots) columns = numberOfSlots;

        int rows = (int)Math.Ceiling((double)(numberOfSlots / columns));
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float slotY = pos.y + marginTopBottom + i * (slotWidthHeight + padding);
                float slotX = pos.x + marginLeftRight + j * (slotWidthHeight + padding);
                Debug.Log("Adding position: x, y, z" + slotX + ", " + slotY + ", " + slotZ);
                slotPositions.Add(new Vector3(slotX, slotY, slotZ));
            }
        }
        Debug.Log("after generation: number" + slotPositions.Count);
    }

    public void setNumberOfSlots(int nr)
    {
        numberOfSlots = nr;
        initSlots();
    }

    public void Clear()
    {
        numberOfSlots = 0;
        slots.Clear();
    }

    public int Find(DragObject obj) {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].GetDragObject() == obj)
                return i;
        }
        return -1;
    }

}