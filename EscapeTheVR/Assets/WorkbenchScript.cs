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

    private List<GameObject> createdContentSlots;
    //private DragObject container; 
    void Start()
    {
        gameboard = FindObjectOfType<GameBoardScript>();
        createdContentSlots = new List<GameObject>();

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

    public GameObject createSlotFromPrefab(Slot slot)
    {
        GameObject instantiated = Instantiate(slotPrefab);
        instantiated.transform.SetParent(gameObject.transform);
        instantiated.transform.position = slot.position;

        return instantiated;
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
        var selectedElement = gameboard.selectedGameObj;
        if (selectedElement == null)
            return (false, null);

        var selectedProgrammigElementType = selectedElement.element.elem;

        if (containerSlotManager.slots[0].isEmpty())
        { 
            // there is no container e.g. var / interval yet
            Debug.Log("ContainerSlot is empty");
            if (selectedProgrammigElementType == programmingElement.elemVar || selectedProgrammigElementType == programmingElement.elemInterval)
            {
                contentSlotManager = new SlotManager(gameObject.transform.position+contentSlotOffset, gameObject.transform.localScale, getNumberOfNeededFillSlots(selectedProgrammigElementType));
                contentSlotManager.initSlots();
                contentSlotManager.slots.ForEach(s => createdContentSlots.Add(createSlotFromPrefab(s)));
                // if selectedElem is a container, checkIf its close enhough to the slot
                return containerSlotManager.handlesElementToSlotRelation(selectedElement);
            }
            else return (false, null);
        }
        else
        {
            // there is a container
            if (selectedProgrammigElementType != programmingElement.elemVar || selectedProgrammigElementType != programmingElement.elemInterval)
            {
                // if selectedElem is a container, checkIf its close enhough to the slot
                return contentSlotManager.handlesElementToSlotRelation(selectedElement);
            }
            else return (false, null);
        }
    }

    public void resetContentSlots()
    {
        var selectedDragObject = gameboard.selectedGameObj;
        if (!selectedDragObject)
            return;

        //var selectedElementType = selectedDragObject.element.elem;
        ////If the removed object is a container object, remove all of the content-objects.
        //if (selectedElementType == programmingElement.elemVar || selectedElementType == programmingElement.elemInterval)
        //{
        //    (bool isCloseEnough, Slot closest) = containerSlotManager.handleElementToSlotRelation(selectedDragObject);
        //    if (!isCloseEnough)
        //    {
                //Remove all items in contentSlots and enable Gravity on the objects...
        contentSlotManager.slots.ForEach(s => {
            if (s.GetDragObject())
            {
                s.GetDragObject().transform.position += new Vector3(0.25f, 0.25f, 0.25f);
                s.GetDragObject().enableGravity();
            }
        });
        contentSlotManager.Clear();

        deleteCreatedSlotPrefabs();
                    //}
                //}
        //else
        //{
        //    (bool isCloseEnough, Slot closest) = contentSlotManager.handleElementToSlotRelation(selectedDragObject);
        //    if (!isCloseEnough && closest != null)
        //        closest.resetElement();
        //}

    }

    private void deleteCreatedSlotPrefabs()
    {
        createdContentSlots.ForEach(slotPrefab =>
        {
            Destroy(slotPrefab.gameObject);
            Destroy(slotPrefab);
        });
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
