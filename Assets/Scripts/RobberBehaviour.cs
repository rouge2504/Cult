using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : MonoBehaviour
{
    BehaviourTree tree;

    public GameObject diamond;
    public GameObject van;
    public GameObject backDoor;
    NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING};
    public ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf goToVan = new Leaf("Go to Van", GoToVan);
        Leaf goToBackDoor = new Leaf("Go to Backdoor", GoToBackDoor);

        steal.AddChild(goToBackDoor);
        steal.AddChild(goToDiamond);
        steal.AddChild(goToVan);
        steal.AddChild(goToBackDoor);
        tree.AddChild(steal);

        /*Node eat = new Node("Eat Something");
        Node pizza = new Node("Go To Pizza Shop");
        Node buy = new Node("Buy Pizza");

        eat.AddChild(pizza);
        eat.AddChild(buy);
        tree.AddChild(eat);*/

        tree.PrintTree();


    }

    public Node.Status GoToDiamond()
    {
        return GoToLocation(diamond.transform.position);
    }

    public Node.Status GoToVan()
    {
        return GoToLocation(van.transform.position);
    }

    public Node.Status GoToBackDoor()
    {
        return GoToLocation(backDoor.transform.position);
    }

    Node.Status GoToLocation(Vector3 destination)
    {
        float distantToTarget = Vector3.Distance(destination, this.transform.position);
        if (state == ActionState.IDLE)
        {
            agent.SetDestination(destination);
            state = ActionState.WORKING;
        }else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }else if  (distantToTarget  < 2)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCES;
        }

        return Node.Status.RUNNING;

    }

    // Update is called once per frame
    void Update()
    {
        if (treeStatus == Node.Status.RUNNING)
        {
            treeStatus = tree.Procces();
        }
    }
}
