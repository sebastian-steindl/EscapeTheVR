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

    public GameObject gameBoard;
    private ElementStone element;

    private void OnMouseDown()
    {
        //offset = gameObject.transform.position - GetMouseWorldPos();
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    }

    private void OnMouseUp()
    {
        // with the GetComponent we can call functions from other scripts
        gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(gameObject, element);
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
