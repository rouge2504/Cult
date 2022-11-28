using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DepSequence : Node
{
    BehaviourTree dependency;
    NavMeshAgent agent;
    public DepSequence(string n, BehaviourTree d, NavMeshAgent a)
    {
        name = n;
        dependency = d;
        agent = a;
    }

    public override Status Procces()
    {

        if (dependency.Procces() == Status.FAILURE)
        {
            agent.ResetPath();
            foreach (Node n in children)
            {
                n.Reset();
            }
            return Status.FAILURE;
        }
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
