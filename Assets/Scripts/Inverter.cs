using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    public Inverter(string n)
    {
        name = n;
    }

    public override Status Procces()
    {
        Status childstatus = children[0].Procces();
        if (childstatus == Status.RUNNING) return Status.RUNNING;
        if (childstatus == Status.FAILURE)
            return Status.SUCCES;
        else
            return Status.FAILURE;

    }


}
