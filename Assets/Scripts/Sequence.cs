using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    public override Status Procces()
    {
        Status childStatus = children[currentChildren].Procces();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.FAILURE) return childStatus;

        currentChildren++;
        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            return Status.SUCCES;
        }

        return Status.RUNNING;
    }
}
