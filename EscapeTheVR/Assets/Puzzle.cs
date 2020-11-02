using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle
{

    string name;
    // todo: if we allow multiple solutions, we need a nested array
    List<ElementStone> solutionGivenByUser; // the solutionGivenByUser: ElementStones which contain programmingElements
    List<programmingElement> correctSolution; // the correct, expected order of programmingElements
    List<int> wrongIndices;

    private int emptySlots; // if you want to provide empty slots, e.g. puzzle needs 4 but you provide 5 slots
    public Puzzle(string puzzleName, int slots = 0)
    {
        name = puzzleName;
        emptySlots = slots;

        solutionGivenByUser = new List<ElementStone>();
        correctSolution = new List<programmingElement>();
        wrongIndices = new List<int>();
    }

    /*
     * shouldHighlight: highlights Borders if true
     * return: true if puzzle is correctly solved, false if not
     */
    public bool evaluatePuzzle(bool shouldHighlight)
    {
        this.wrongIndices = checkOrder();

        //If no element is inside the GameBoard, just retrun false, since puzzle is not solved.
        if (wrongIndices == null) return false;

        if (shouldHighlight)
        {
            for (int i = 0; i < solutionGivenByUser.Count; i++)
            {
                //if elements not placed in the GameBoard, ignore them...
                if (solutionGivenByUser.ElementAt(i) == null) continue;

                if (wrongIndices.Contains(i)) 
                    solutionGivenByUser.ElementAt(i).redBorder();
                
                else solutionGivenByUser.ElementAt(i).greenBorder();
            }
        }

        return wrongIndices.Count == 0;
    }

    public void setSolution(List<programmingElement> solutionInCorrectOrder)
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
            if (solutionGivenByUser[i].elem != correctSolution[i]) 
            {
                wrongIndices.Add(i);
            }
        }
        return wrongIndices;
    }

   public int getNumberOfSlots()
    {
        return solutionGivenByUser.Count + emptySlots;
    }
}
