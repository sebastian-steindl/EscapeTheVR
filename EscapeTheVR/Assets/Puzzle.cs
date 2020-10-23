using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle
{
    ElementStone[] elementStones; // the given order of programmingElements
    programmingElement[] elementOrder; // the correct, expected order of programmingElements
    List<int> wrongIndices;

    private int emptySlots; // if you want to provide empty slots, e.g. puzzle needs 4 but you provide 5 slots
    public Puzzle()
    {
        emptySlots = 0;
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
            for (int i = 0; i < elementStones.Length; i++)
            {
                if (wrongIndices.Contains(i)) 
                    elementStones[i].redBorder();
                
                else elementStones[i].greenBorder();
            }

        }

        return wrongIndices.Count == 0;
    }

    /*
     If wrongIndices.Count == 0 everything is fine, else those are the indices that are wrongly placed
    could be highlighted etc in VR
     */
    List<int> checkOrder()
    {
        List<int> wrongIndices = new List<int>();

        for (int i = 0; i < elementOrder.Length; i++)
        {
            // this should also work if their length is different, e.g. 4 stones are needed
            // but only 3 are placed. 
            if (elementStones[i].elem != elementOrder[i]) 
            {
                wrongIndices.Add(i);
            }
        }
        return wrongIndices;
    }

   public int getNumberOfSlots()
    {
        return elementStones.Length + emptySlots;
    }
}
