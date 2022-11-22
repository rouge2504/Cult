using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Node
{
    public delegate Status Tick();
    public Tick ProccesMethod;

    public Leaf() { }

    public Leaf(string n, Tick pm)
    {
        name = n;
        ProccesMethod = pm;
    }
    public override Status Procces()
    {
        if (ProccesMethod != null)
            return ProccesMethod();
        return Status.FAILURE;
    }
}
