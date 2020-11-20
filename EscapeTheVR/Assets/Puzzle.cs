using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle
{

    string name;
    // todo: if we allow multiple solutions, we need a nested array
    List<ElementStone> solutionGivenByUser; // the solutionGivenByUser: ElementStones which contain programmingElements
    List<int> correctSolution; // the correct, expected order of programmingElements
    List<int> wrongIndices;

    private int emptySlots; // if you want to provide empty slots, e.g. puzzle needs 4 but you provide 5 slots
    private bool snapEnabled;

    public Puzzle(string puzzleName, int slots = 0, bool snapEnabled= false)
    {
        name = puzzleName;
        emptySlots = slots;
        this.snapEnabled = snapEnabled;

        solutionGivenByUser = new List<ElementStone>();
        correctSolution = new List<int>();
        wrongIndices = new List<int>();
    }

    /*
     * shouldHighlight: highlights Borders if true
     * return: true if puzzle is correctly solved, false if not
     */
    public bool evaluatePuzzle()
    {
        this.wrongIndices = checkOrder();

        //If no element is inside the GameBoard, just retrun false, since puzzle is not solved.
        if (wrongIndices == null) return false;
        return wrongIndices.Count == 0;
    }

    public void setSolution(List<int> solutionInCorrectOrder)
    {
        this.correctSolution = solutionInCorrectOrder;
    }

    public void setUserSolution(List<ElementStone> solutionGivenByUser) { this.solutionGivenByUser = solutionGivenByUser; }

    /*
     If wrongIndices.Count == 0 everything is fine, else those are the indices that are wrongly placed
    could be highlighted etc in VR
    Returns null when no element is inside the GameBoard.
     */
    List<int> checkOrder()
    {
        List<int> wrongIndices = new List<int>();
        Debug.Log("Puzzle->checkOrder->user:");
        solutionGivenByUser.ForEach(es => {
            if (es == null) Debug.Log("Slot not populated");
            else Debug.Log(es.elem);
        });
        Debug.Log("Puzzle->checkOrder->correct:");
        correctSolution.ForEach(es => Debug.Log(es.ToString()));
        Debug.Log("----------------------------------------------");

        if (solutionGivenByUser.Count == 0) return null;
        for (int i = 0; i < correctSolution.Count; i++)
        {
            // when the current element hasn't yet been moved to a slot, or has been removed from the slot.
            if (solutionGivenByUser[i] == null) {
                wrongIndices.Add(i);
                continue;
            }

            // this should also work if their length is different, e.g. 4 stones are needed
            // but only 3 are placed. 
            if (solutionGivenByUser[i].id != correctSolution[i]) 
            {
                wrongIndices.Add(i);
            }
        }
        return wrongIndices;
    }

   public int getNumberOfSlots()
    {
        return correctSolution.Count;
    }

    public bool isSnapEnabled() { return snapEnabled; }
}
