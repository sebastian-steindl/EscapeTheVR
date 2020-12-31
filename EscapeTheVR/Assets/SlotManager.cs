using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;

public class SlotManager
{
    private int numberOfSlots;
    public List<Vector3> slotPositions;
    private float slotWidthHeight = 0.3f;
    public List<Slot> slots { get; }

    // vr gameboard position and scale
    public Vector3 pos;
    public Vector3 scale;

    public float marginTopBottom = 0.35f;
    public float marginLeftRight = 0.25f;

    private float slotZOffset = 1.2f;

    public SlotManager(Vector3 pos, Vector3 scale, int slotNum = 1, float slotZOffset = 1.2f)
    {
        this.pos = pos;
        this.scale = scale;

        slots = new List<Slot>();
        slotPositions = new List<Vector3>();
        numberOfSlots = slotNum;
        this.slotZOffset = slotZOffset;
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

    internal void initIntervalSlots(Vector3 containerSlotPosition)
    {
        slots.Clear(); //empty list

        float padding = 1.5f * slotWidthHeight;

        float availableWidth = scale.x - 2 * marginLeftRight;

        float slotZ = pos.z + this.slotZOffset;
        float slotY = pos.y;
        double columns = Math.Floor(availableWidth / (slotWidthHeight + padding));

        float slotX_1 = pos.x + 2 * (slotWidthHeight + padding); //- * (slotWidthHeight + padding);
        float slotX_2 = pos.x - 0f * (slotWidthHeight + padding);

        slotPositions.Add(new Vector3(slotX_1, slotY, slotZ));
        slotPositions.Add(new Vector3(slotX_2, slotY, slotZ));
        
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

        float availableWidth = scale.x - 2 * marginLeftRight;
      
        float slotZ = pos.z + this.slotZOffset;

        double columns = Math.Floor(availableWidth / (slotWidthHeight + padding));
        Debug.Log("++++++++++++++++++++POS " + pos);
        if (columns > numberOfSlots) columns = numberOfSlots;
        int rows = (int)Math.Ceiling((double)(numberOfSlots / columns));
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float slotY = pos.y - i * (slotWidthHeight + padding);
                float slotX = pos.x - j * (slotWidthHeight + padding);
                Debug.Log("Adding position: x, y, z" + slotX + ", " + slotY + ", " + slotZ);
                slotPositions.Add(new Vector3(slotX, slotY, slotZ));
            }
            columns = numberOfSlots - columns * (i+1)>= columns ? columns : numberOfSlots - columns * (i + 1);
        }
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