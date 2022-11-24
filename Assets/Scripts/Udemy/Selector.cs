using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(string n)
    {
        name = n;
    }

    public override Status Procces()
    {
        Status childStatus = children[currentChildren].Procces();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.SUCCES)
        {
            currentChildren = 0;
            return Status.SUCCES;
        }


        currentChildren++;

        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            return Status.FAILURE;
        }
        return Status.RUNNING;
    }
}
