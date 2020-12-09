using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class WorkbenchScript : MonoBehaviour
{

    private static readonly string ContentText = "WorkbenchContent";
    private static readonly string ContentHeader = "WorkbenchHeader";

    public GameObject slotPrefab;
    public SlotManager containerSlotManager; // this manages the first slot that is always active, and decides how many other slots appear
    public SlotManager contentSlotManager;
    Vector3 contentSlotOffset;
    private GameBoardScript gameboard;
    private List<GameObject> createdContentSlots;    

    void Start()
    {
        gameboard = FindObjectOfType<GameBoardScript>();
        createdContentSlots = new List<GameObject>();

        containerSlotManager = new SlotManager(gameObject.transform.position, gameObject.transform.localScale, 1);
        contentSlotOffset = new Vector3(0,0,0.5f);
        containerSlotManager.initSlots(); 
        containerSlotManager.slots.ForEach(s => createSlotFromPrefab(s));
        contentSlotManager = new SlotManager(gameObject.transform.position + contentSlotOffset, gameObject.transform.localScale, 0);
        updateText();
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
            (bool isCloseEnough, Slot closest) = containerSlotManager.handlesElementToSlotRelation(selectedElement);
            if (isCloseEnough && (selectedProgrammigElementType == programmingElement.elemVar || selectedProgrammigElementType == programmingElement.elemInterval))
            {
                contentSlotManager = new SlotManager(gameObject.transform.position+contentSlotOffset, gameObject.transform.localScale, getNumberOfNeededFillSlots(selectedProgrammigElementType));
                contentSlotManager.initSlots();
                contentSlotManager.slots.ForEach(s => createdContentSlots.Add(createSlotFromPrefab(s)));
                // if selectedElem is a container, checkIf its close enhough to the slot
                updateText();
                return containerSlotManager.handlesElementToSlotRelation(selectedElement);
            }
            else return (false, null);
        }
        else
        {
            // Content slots only allow boolean, number and text values. 
            if (selectedProgrammigElementType == programmingElement.elemBool || selectedProgrammigElementType == programmingElement.elemText || selectedProgrammigElementType == programmingElement.elemNumber)
            {
                (bool isCloseEnough, Slot closest) = contentSlotManager.handlesElementToSlotRelation(selectedElement);
                if (isCloseEnough)
                {

                    var containerElement = containerSlotManager.slots[0].getElement();

                    // Update Var element
                    if (containerElement.elem == programmingElement.elemVar)
                    {
                        Debug.Log("Update Variable Stome...");
                        VariableStone stone = (VariableStone)containerElement;
                        stone.filledWith = selectedElement.element;
                        containerElement = stone;
                    }
                    else if (containerElement.elem == programmingElement.elemInterval)
                    { //Code for updating InvervalStones / elements
                        Debug.Log("Update Variable Stome...");

                        //Interval only works with Numberstones...
                        if (selectedProgrammigElementType != programmingElement.elemNumber)
                        {
                            Console.Error.Write("Tried to update an IntervalStone with a non numeric stone element!");
                            return (false, null);
                        }

                        IntervalStone stone = (IntervalStone)containerElement;
                        if (closest == contentSlotManager.slots[0])
                            stone.from = selectedElement.element;
                        else
                            stone.to = selectedElement.element;
                        containerElement = stone;

                    }


                }
                updateText();
                // if selectedElem is a container, checkIf its close enhough to the slot
                return (isCloseEnough, closest);
            }
            else
            {
                bool isContainerElement = containerSlotManager.Find(selectedElement) != -1;
                (bool isCloseEnough, Slot closest) = containerSlotManager.handlesElementToSlotRelation(selectedElement);
                if (!isCloseEnough && isContainerElement)
                {
                    Debug.Log("Reset Containerslot.");
                    resetContentSlots();
                    updateText();
                }
                return (isCloseEnough, closest);
            }
        }
    }

    public void resetContentSlots()
    {
        var selectedDragObject = gameboard.selectedGameObj;
        if (!selectedDragObject)
            return;

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

    private void updateText() {
        StringBuilder sb = new StringBuilder() ;
        if (!containerSlotManager.slots[0].isEmpty())
        {
            var element = containerSlotManager.slots[0].getElement();

            //Make the descition of what thype the current element actually is.
            if (element.elem == programmingElement.elemInterval)
            {
                var stone = element as IntervalStone;
                sb.Append("Interval von ");
                if (stone.from == null)
                    sb.Append("<Leer>");
                else
                    sb.Append(stone.from.descriptionText);
                sb.Append(" bis ");
                if (stone.to == null)
                    sb.Append("<Leer>");
                else
                    sb.Append(stone.to.descriptionText);
            }
            else
            {
                //Since the containerSlotManager only accepts intervals and vars, we can assume that here we have an element of the type VariableStone.
                var stone = element as VariableStone;
                if (stone.filledWith == null)
                    sb.Append("<Leer>");
                else
                    sb.Append(stone.filledWith.descriptionText);
            }

            //If the text fields are hidden, show them.
            if (!GameObject.Find(ContentHeader).GetComponent<Renderer>().enabled)
            {
                GameObject.Find(ContentHeader).GetComponent<Renderer>().enabled=true;
                GameObject.Find(ContentText).GetComponent<Renderer>().enabled=true;
            }

            //Set the text.
            GameObject.Find(ContentText).GetComponent<TextMeshPro>().text = sb.ToString() ;
        }
        else
        {
            //Do not show the Content Text...
            GameObject.Find(ContentHeader).GetComponent<Renderer>().enabled = false;
            GameObject.Find(ContentText).GetComponent<Renderer>().enabled = false;
        }
        

    }
}
