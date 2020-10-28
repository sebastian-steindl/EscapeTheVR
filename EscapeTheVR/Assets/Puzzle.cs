using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        if (shouldHighlight)
        {
            for (int i = 0; i < solutionGivenByUser.Count; i++)
            {
                if (wrongIndices.Contains(i)) 
                    solutionGivenByUser.ElementAt(i).redBorder();
                
                else solutionGivenByUser.ElementAt(i).greenBorder();
            }
        }

        return wrongIndices.Count == 0;
    }

    public void setSolution(List<ElementStone> solutionInCorrectOrder)
    {
        this.solutionGivenByUser = solutionInCorrectOrder;
    }

    /*
     If wrongIndices.Count == 0 everything is fine, else those are the indices that are wrongly placed
    could be highlighted etc in VR
     */
    List<int> checkOrder()
    {
        List<int> wrongIndices = new List<int>();

        for (int i = 0; i < correctSolution.Count; i++)
        {
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
