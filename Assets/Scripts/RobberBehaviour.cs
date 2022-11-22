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
    public GameObject frontDoor;
    NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING};
    public ActionState state = ActionState.IDLE;

    Node.Status treeStatus = Node.Status.RUNNING;

    [Range(0, 1000)]
    public int money = 800;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond);
        Leaf hasGotMoney = new Leaf("Has got Money", HasMoney);
        Leaf goToVan = new Leaf("Go to Van", GoToVan);
        Leaf goToBackDoor = new Leaf("Go to Backdoor", GoToBackDoor);
        Leaf goToFrontDoor = new Leaf("Go to Frontdoor", GoToFrontDoor);
        Selector openDoor = new Selector("Open Door");

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);
        steal.AddChild(hasGotMoney);
        steal.AddChild(openDoor);
        //steal.AddChild(goToBackDoor);
        steal.AddChild(goToDiamond);
        //steal.AddChild(goToBackDoor);
        steal.AddChild(goToVan);
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
        Node.Status s = GoToLocation(diamond.transform.position);

        if (s == Node.Status.SUCCES)
        {
            diamond.transform.parent = this.gameObject.transform;
        }

        return s;
    }
    public Node.Status HasMoney()
    {
        if (money >= 500)
        {
            return Node.Status.FAILURE;
        }
        
        return Node.Status.SUCCES;
    }
    public Node.Status GoToVan()
    {

        Node.Status s = GoToLocation(van.transform.position);

        if (s == Node.Status.SUCCES)
        {
            diamond.gameObject.SetActive(false);
            money += 500;
        }
        return s;
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }
    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCES)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.SetActive(false);
                return Node.Status.SUCCES;
            }
            return Node.Status.FAILURE;
        }
        else
        {
            return s;
        }
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
        if (treeStatus != Node.Status.SUCCES)
        {
            treeStatus = tree.Procces();
        }
    }
}
