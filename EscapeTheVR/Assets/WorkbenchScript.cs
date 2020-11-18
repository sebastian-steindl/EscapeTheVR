using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchScript : MonoBehaviour
{
    public GameObject slotPrefab;

    public SlotManager containerSlotManager; // this manages the first slot that is always active, and decides how many other slots appear
    public SlotManager contentSlotManager;

    Vector3 contentSlotOffset;
    private GameBoardScript gameboard;
    //private DragObject container; 
    void Start()
    {
        gameboard = FindObjectOfType<GameBoardScript>();

        containerSlotManager = new SlotManager(gameObject.transform.position, gameObject.transform.localScale, 1);
        contentSlotOffset = new Vector3(0,0,0.5f);
        containerSlotManager.initSlots(); 
        containerSlotManager.slots.ForEach(s => createSlotFromPrefab(s));
        contentSlotManager = new SlotManager(gameObject.transform.position + contentSlotOffset, gameObject.transform.localScale, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Slot createSlotFromPrefab(Slot slot)
    {
        GameObject instantiated = Instantiate(slotPrefab);
        instantiated.transform.position = slot.position;
        return null;
    }
    private void OnMouseEnter()
    {
        //Do we want to light up the GameBoard when programmingElement enters 
    }

    private void OnMouseExit()
    {
    }

    private void OnMouseOver()
    {
        if (!gameboard.selectedGameObj) return;

        setSelectedElementToSlotIfCloseEnough();
    }


    /// <summary>
    /// This function sets the currently selected object to the closest slot, if it is close enough.
    /// Attention: It also resets the current object from the slot, if it is still set eventhogh the object isn't close enough.
    /// </summary>
    /// <returns></returns>
    public (bool isCloseEnough, Slot closestSlot) setSelectedElementToSlotIfCloseEnough()
    {
        var selectedElement = gameboard.getSelectedElement();
        if (selectedElement == null)
            return (false, null);

        if (containerSlotManager.slots[0].isEmpty())
        { 
            // there is no container e.g. var / interval yet
            Debug.Log("ContainerSlot is empty");
            if (selectedElement.elem == programmingElement.elemVar || selectedElement.elem == programmingElement.elemInterval)
            {
                contentSlotManager = new SlotManager(gameObject.transform.position+contentSlotOffset, gameObject.transform.localScale, getNumberOfNeededFillSlots(selectedElement.elem));
                contentSlotManager.initSlots();
                contentSlotManager.slots.ForEach(s => createSlotFromPrefab(s));
                // if selectedElem is a container, checkIf its close enhough to the slot
                return containerSlotManager.handlesElementToSlotRelation(gameboard.selectedGameObj, selectedElement);
            }
            else return (false, null);
        }
        else
        {
            // there is a container
            if (selectedElement.elem != programmingElement.elemVar || selectedElement.elem != programmingElement.elemInterval)
            {
                // if selectedElem is a container, checkIf its close enhough to the slot
                return contentSlotManager.handlesElementToSlotRelation(gameboard.selectedGameObj, selectedElement);
            }
            else return (false, null);
        }
    }

    private int getNumberOfNeededFillSlots(programmingElement elementType)
    {
        switch (elementType)
        {
            case programmingElement.elemVar:
                return 1;
            case programmingElement.elemInterval:
                return 2;
            default:
                return -1;
        }
    }
}
