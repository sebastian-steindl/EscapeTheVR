using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTriggerBehavior : MonoBehaviour
{

    private GameObject currentCollider = null;
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
        if (currentCollider == other.gameObject) return;
        currentCollider = other.gameObject;
        var dragObject = other.gameObject.GetComponent<DragObject>();

        // Only snap object into slot if it is not currently being dragged.
        if (dragObject != null && !dragObject.IsBeingDragged)
        {
            dragObject.gameBoard.GetComponent<GameBoardScript>().registerSelectedElement(dragObject);
            Debug.Log("Trigger: " + other.gameObject.GetComponent<Rigidbody>().isKinematic);
            dragObject.onButtonUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentCollider == other.gameObject)
        {
            Debug.Log("Exitting Collider.");
            currentCollider = null;

            // Check if you are closer to the workbench than the gameboard, since we need to reset the workbench, if you leave it with the current element.
            // Yes I know, this is ugly but it should work. Hopefully.
            var workbench = FindObjectOfType<WorkbenchScript>();
            SlotManager gameBoardSlotManager = FindObjectOfType<GameBoardScript>().GetGameBoard();
            SlotManager wbContainerSM = workbench.containerSlotManager;
            var currentSelection = other.gameObject.GetComponent<DragObject>();
            float closestWbContainerDistance = wbContainerSM.getClostestDistance(currentSelection);
            float closestGameBoardDistance = gameBoardSlotManager.getClostestDistance(currentSelection);
            if (closestWbContainerDistance < closestGameBoardDistance)
            {
                workbench.setSelectedElementToSlotIfCloseEnough();
            }
        }
    }
}
