using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventar : MonoBehaviour
{

    List<DragObject> storedObjects;
    DragObject currSel;
    bool isInside = false;
    
    // Start is called before the first frame update
    void Start()
    {
        storedObjects = new List<DragObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCurrentlySelectedElement(DragObject obj) {
        currSel = obj;
    }

    /// <summary>
    /// returns if the pointer is currently inside the element
    /// </summary>
    public bool mouseUpFunction() {
        if (isInside) {
            storedObjects.Add(currSel);
            Debug.Log("Added Element: " + currSel.programmingElementType+"\tName: "+currSel.name);
        }
        else if (storedObjects.Contains(currSel)) //If an element was draged out of the storage... ->reactivate Gravety
            storedObjects.Remove(currSel);
        resetCurrentlySelectedElement();
        return isInside;
    }

    public void resetCurrentlySelectedElement() {
        currSel = null;
    }

    private void OnMouseEnter()
    {
        isInside = true;
        Debug.Log("Yo bin drin...");
    }

    private void OnMouseExit()
    {
        isInside = false;
        Debug.Log("... und raus bist du!");
    }

    private void OnMouseOver()
    {
        Debug.Log("mhhh...");
    }
}
