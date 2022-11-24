using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSelector : Node
{

    bool shuffled = false;
    public RSelector(string n)
    {
        name = n;
    }

    public override Status Procces()
    {
        if (!shuffled)
        {
            children.Shuffle();
            shuffled = true;
        }
        Status childStatus = children[currentChildren].Procces();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.SUCCES)
        {
            currentChildren = 0;
            shuffled = false;
            return Status.SUCCES;
        }


        currentChildren++;

        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            shuffled = false;
            return Status.FAILURE;
        }
        return Status.RUNNING;
    }
}
