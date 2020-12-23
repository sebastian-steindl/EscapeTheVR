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
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMouseDown()
    {
        if (!gameboard) gameboard = FindObjectOfType<GameBoardScript>();
        gameboard.resetSlots();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Sphere (2)")
        {
            OnMouseDown();
            lastCollider = other;
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
