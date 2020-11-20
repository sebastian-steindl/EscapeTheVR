using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle
{

    string name;
    // todo: if we allow multiple solutions, we need a nested array
    List<ElementStone> solutionGivenByUser; // the solutionGivenByUser: ElementStones which contain programmingElements
    List<ElementStone> flattenedCorrectSolution; // the correct, expected order of programmingElements
    List<ElementStone> unflattenedCorrectSolution;
    List<int> wrongIndices;

    private int emptySlots; // if you want to provide empty slots, e.g. puzzle needs 4 but you provide 5 slots
    private bool snapEnabled;

    public Puzzle(string puzzleName, int slots = 0, bool snapEnabled= false)
    {
        name = puzzleName;
        emptySlots = slots;
        this.snapEnabled = snapEnabled;

        solutionGivenByUser = new List<ElementStone>();
        flattenedCorrectSolution = new List<ElementStone>();
        unflattenedCorrectSolution = new List<ElementStone>();
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

    public void setSolution(List<ElementStone> solutionInCorrectOrder)
    {
        unflattenedCorrectSolution = solutionInCorrectOrder;
        flattenedCorrectSolution = flattenSolution(solutionInCorrectOrder);
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
        List<ElementStone> flattenedUserSolution = new List<ElementStone>();
        flattenedUserSolution = flattenSolution(solutionGivenByUser);
        Debug.Log("Puzzle->checkOrder->correct:");
        flattenedCorrectSolution.ForEach(es => Debug.Log(es.ToString()));
        Debug.Log("----------------------------------------------");


        if (flattenedUserSolution.Count == 0) return null;
        for (int i = 0; i < flattenedCorrectSolution.Count; i++)
        {
            // when the current element hasn't yet been moved to a slot, or has been removed from the slot.
            if (flattenedUserSolution[i] == null)
            {
                wrongIndices.Add(i);
                continue;
            }

            // this should also work if their length is different, e.g. 4 stones are needed
            // but only 3 are placed. 
            if (flattenedUserSolution[i].id != flattenedCorrectSolution[i].id)
            {
                wrongIndices.Add(i);
            }
        }
        return wrongIndices;
    }

    private List<ElementStone> flattenSolution(List<ElementStone> unflattedList)
    {
        List<ElementStone> flattened = new List<ElementStone>();
        unflattedList.ForEach(es =>
        {
            if (es is VariableStone)
            {
                flattened.Add(es);
                flattened.Add((es as VariableStone).filledWith);
            }
            else if (es is IntervalStone)
            {
                flattened.Add(es);
                flattened.Add((es as IntervalStone).from);
                flattened.Add((es as IntervalStone).to);
            }
            else flattened.Add(es);
        });

        return flattened;
    }

    public int getNumberOfSlots()
    {
        return unflattenedCorrectSolution.Count;
    }

    public bool isSnapEnabled() { return snapEnabled; }
}
