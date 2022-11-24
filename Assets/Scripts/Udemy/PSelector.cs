using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSelector : Node
{

    Node[] nodeArray;
    bool ordered = false;
    public PSelector(string n)
    {
        name = n;
    }

    void OrderNodes()
    {
        nodeArray = children.ToArray();
        Sort(nodeArray, 0, children.Count - 1);
        children = new List<Node>(nodeArray);
    }

    int Partition(Node[] array, int low,  int high)
    {
        OrderNodes();
        Node pivot = array[high];

        int lowIndex = (low - 1);

        //2. Reorder the collection.
        for (int j = low; j < high; j++)
        {
            if (array[j].sortOrder <= pivot.sortOrder)
            {
                lowIndex++;

                Node temp = array[lowIndex];
                array[lowIndex] = array[j];
                array[j] = temp;
            }
        }

        Node temp1 = array[lowIndex + 1];
        array[lowIndex + 1] = array[high];
        array[high] = temp1;

        return lowIndex + 1;
    }

    void Sort(Node[] array, int low, int high)
    {
        if (low < high)
        {
            int partitionIndex = Partition(array, low, high);

            //3. Recursively continue sorting the array
            Sort(array, low, partitionIndex - 1);
            Sort(array, partitionIndex + 1, high);
        }
    }

    public override Status Procces()
    {
        if (!ordered)
        {
            OrderNodes();
            ordered = true;
        }
        Status childStatus = children[currentChildren].Procces();
        if (childStatus == Status.RUNNING) return Status.RUNNING;
        if (childStatus == Status.SUCCES)
        {
            children[currentChildren].sortOrder = 1;
            currentChildren = 0;
            ordered = false;
            return Status.SUCCES;
        }
        //else
        //{
        //    children[currentChildren].sortOrder = 10;

        //}


        currentChildren++;

        if (currentChildren >= children.Count)
        {
            currentChildren = 0;
            ordered = false;
            return Status.FAILURE;
        }
        return Status.RUNNING;
    }
}
