using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProccesMethod;

    public delegate Status TickM(int val);
    public TickM ProccesMethodM;

    public int index;

    public Leaf() { }

    public Leaf(string n, Tick pm)
    {
        name = n;
        ProccesMethod = pm;
    }

    public Leaf(string n, int i, TickM pm)
    {
        name = n;
        ProccesMethodM = pm;
        index = i;
    }

    public Leaf(string n, Tick pm, int order)
    {
        name = n;
        ProccesMethod = pm;
        sortOrder = order;
    }
    public override Status Procces()
    {
        if (ProccesMethod != null)
            return ProccesMethod();
        else if (ProccesMethodM != null)
            return ProccesMethodM(index);
        return Status.FAILURE;
    }
}
