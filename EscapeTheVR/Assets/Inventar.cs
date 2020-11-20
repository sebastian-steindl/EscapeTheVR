using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventar : MonoBehaviour, IPointerClickHandler,
    IDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    List<InventorySlot> inventorySlots;
    //List<DragObject> storedObjects;
    DragObject currSel;
    public Inventar instance;
    public ElementStone draggedOutsideItem;
    // Start is called before the first frame update
    void Start()
    {
        //storedObjects = new List<DragObject>();
        instance = this;
        inventorySlots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrentlySelectedElement(DragObject obj) {
        currSel = obj;
    }

    public void resetCurrentlySelectedElement() {
        currSel = null;
    }

    public void addItemToEmptySlot(ElementStone elementStone)
    {
        // add item to the first slot that is empty
        // then the slot will update its image
        getNextEmptySlot().addItem(elementStone);

        // since the elem is now in the slot, we can remove it from the scene
        Destroy(currSel.gameObject);
        Destroy(currSel);
    }

    private InventorySlot getNextEmptySlot()
    {
        return inventorySlots.Find(invSlot => invSlot.item == null);
    }
    

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("dragging outwards");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //If you enter with an element, make shure it is removed from the GameBoard.
        FindObjectOfType<GameBoardScript>().setSelectedElementToSlotIfCloseEnough();

        if (currSel != null)
        {
            addItemToEmptySlot(currSel.element);
            Debug.Log("Enter");
        }
        else Debug.Log("addItem is inside but curSel == null");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(draggedOutsideItem != null)
        {
            createElementFromPrefab(eventData.position);
            draggedOutsideItem = null;
        }
    }


    private void createElementFromPrefab(Vector3 pos)
    {
        //TODO anpassungen sodass richtig erzeugt wird: Farbe etc.
        GameObject instantiated = Instantiate(getPrefabForPuzzleProgrammingElement());
        instantiated.transform.position = Vector3.Scale(new Vector3(3f, 3f,3f), Camera.main.transform.forward) + Camera.main.ScreenToWorldPoint(pos);
        instantiated.GetComponent<DragObject>().gameBoard = FindObjectOfType<GameBoardScript>().gameObject;
        instantiated.GetComponent<DragObject>().element.id = draggedOutsideItem.id;
        instantiated.GetComponent<DragObject>().disableGravity();
    }

    private GameObject getPrefabForPuzzleProgrammingElement()
    {

        switch (draggedOutsideItem.elem)
        {
            case programmingElement.elemFuncPrint:
                return Resources.Load("prefabPrintElement", typeof(GameObject)) as GameObject;
            case programmingElement.elemVar:
                return Resources.Load("prefabVarElement", typeof(GameObject)) as GameObject;
            default:
                return Resources.Load("prefabVarElement", typeof(GameObject)) as GameObject;
        }
    }

}
