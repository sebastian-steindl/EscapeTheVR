using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTriggerBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var dragObject = other.gameObject.GetComponent<DragObject>();

        // Only snap object into slot if it is not currently being dragged.
        if (dragObject != null && !dragObject.IsBeingDragged)
        {
            dragObject.gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(dragObject);
            dragObject.OnMouseUp();
        }
    }
}
