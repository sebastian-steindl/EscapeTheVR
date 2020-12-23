using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonResetGameboard : MonoBehaviour
{
    // Start is called before the first frame update

    private GameBoardScript gameboard;
    private Collider lastCollider = null;
    void Start()
    {
        gameboard = FindObjectOfType<GameBoardScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMouseDown()
    {
        //gameboard.resetSlots();
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Debug.Log(other.name);
        if (other.name == "Sphere (2)")
        {
            OnMouseDown();
            lastCollider = other;
        }
        else
        {
            Debug.LogWarning("IS THIS CORRECT? The name was not Spehere (2) but still calling OnMouseDown()");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (lastCollider == other)
        {
            lastCollider = null;
        }
    }
}
