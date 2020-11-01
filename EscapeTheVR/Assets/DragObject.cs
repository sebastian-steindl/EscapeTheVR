using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 https://www.youtube.com/watch?v=0yHBDZHLRbQ
 */
public class DragObject : MonoBehaviour
{
    //private Vector3 offset;
    private float zCoord;
    private float xCoord;
    public GameObject gameBoard;

    public string programmingElementType;
    private ElementStone element;

    private void Awake()
    {
        element = ElementStoneFactory.Instance.createElementStone(programmingElementType);

        gameObject.GetComponent<Renderer>().material = MaterialLoader.Instance.getMaterial(programmingElementType);
        
    }

    private void OnMouseDown()
    {
        //offset = gameObject.transform.position - GetMouseWorldPos();
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // with the GetComponent we can call functions from other scripts
        gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(gameObject, element);
    }

    private void OnMouseUp()
    {
        gameBoard.GetComponent<GameBoardScript>().resetSelectedElement();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos();// + offset;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
