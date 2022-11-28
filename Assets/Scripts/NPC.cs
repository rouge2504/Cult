using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPC : BTAgent
{
    public enum Status { IDLE, WALKING};

    public Status status = Status.IDLE;

    public GameObject[] points;

    public GameObject pointsContent;

    private Animator animator;


    new void Start()
    {
        base.Start();
        agent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();

        tree = new BehaviourTree();

        Sequence wander = new Sequence("Wander");

        RSelector selectObject = new RSelector("Select Object To Move");

        points = new GameObject[pointsContent.transform.childCount];

        for (int i = 0; i < pointsContent.transform.childCount; i++)
        {
            points[i] = pointsContent.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < points.Length; i++)
        {
            Leaf gta = new Leaf("Go to: " + points[i].name, i, GoToPoint);
            selectObject.AddChild(gta);
        }




        wander.AddChild(selectObject);

        tree.AddChild(wander);

        tree.PrintTree();

    }

    public Node.Status GoToPoint(int i)
    {
        status = Status.WALKING;
        if (!points[i].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(points[i].transform.position);

        if (s == Node.Status.SUCCES)
        {
            status = Status.IDLE;
            //points[i].transform.parent = this.gameObject.transform;
            //pickUp = points[i];
        }

        return s;
    }


    void Update()
    {
        switch (status)
        {
            case Status.IDLE:
                animator.SetBool("isWalking", false);
                break;
            case Status.WALKING:
                animator.SetBool("isWalking", true);
                break;
        }
    }



}
