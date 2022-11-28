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
        if (childStatus == Status.FAILURE)
        {
            currentChildren = 0;
            foreach (Node n in children)
            {
                n.Reset();
            }
            return Status.FAILURE;
        }

        currentChildren++;
        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            return Status.SUCCES;
        }

        return Status.RUNNING;
    }
}
