using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public List<CodeElement> codeElements;
    public string output;

    //Don't even try to touch it!
    public bool isSolved = false;
    public Puzzle(string puzzleName, List<CodeElement> codeElements, int slots = 0, string outputStr = "", bool snapEnabled = false)
    {
        name = puzzleName;
        emptySlots = slots;
        this.snapEnabled = snapEnabled;
        this.codeElements = codeElements;
        output = outputStr;
        
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
        
        //Set solved flag to true
        if (wrongIndices.Count == 0)
            isSolved = true;

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
        for (int i = 0; i < flattenedUserSolution.Count; i++)
        {
            // when the current element hasn't yet been moved to a slot, or has been removed from the slot.
            if (flattenedUserSolution[i] == null)
            {
                // This is the case if a slot is not correctly populated.
                if (flattenedCorrectSolution[i]!=null)
                    wrongIndices.Add(i);
                
                //The else case is when we have a variable element, which does not need to be initialized / has no direct value.
                continue;
            }

            // this should also work if their length is different, e.g. 4 stones are needed
            // but only 3 are placed. 
            if (flattenedUserSolution[i].id != flattenedCorrectSolution[i].id)
            {
                wrongIndices.Add(i);
            }
        }

        // User solution has less elements than correct solution. This is the case, if a variable/interval stone has not yet been populated with a value.
        // Adding the difference in indices to the wrong indices list - imo this should not be necessary because the elements than should be in different order.
        // Nevertheless to be absolutely sure i will add them to the wrong indices list.
        for (int i = flattenedUserSolution.Count; i < flattenedCorrectSolution.Count; i++)
            wrongIndices.Add(i);

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

    public string createCodeText(int stoneId = -1)
    {
        StringBuilder sb = new StringBuilder();
        foreach (CodeElement e in codeElements)
        {
            if (e.elementId == stoneId)
                sb.Append("<color=").Append(Constants.colorHighlight).Append(">").Append(e.codeElement).Append("</color>") ;
            else
                sb.Append(e.codeElement);
            if (e.newLine)
                sb.Append("\n");
        }
        return sb.ToString();
    }
}
