using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour,
    IDragHandler
{
    // Start is called before the first frame update
    public ElementStone item;

    public Image icon;
    
    void Start()
    {
        icon= gameObject.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItem(ElementStone elementStone)
    {
        item = elementStone;
        GetComponentsInChildren<Image>()[GetComponentsInChildren<Image>().Length-1].sprite = item.icon;
        GetComponentsInChildren<Image>()[GetComponentsInChildren<Image>().Length - 1].enabled = true;   
    }

    public void clearItem()
    {
        item = null;
        var background = GetComponentsInChildren<Image>();
        GetComponentsInChildren<Image>()[GetComponentsInChildren<Image>().Length - 1].sprite = null;
        GetComponentsInChildren<Image>()[GetComponentsInChildren<Image>().Length - 1].enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            gameObject.GetComponentInParent<Inventar>().draggedOutsideItem = item;
            clearItem();
        }
    }
}
