using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{


    public GameObject diamond;
    public GameObject painting;
    public GameObject van;
    public GameObject backDoor;
    public GameObject frontDoor;

    public GameObject[] art;

    GameObject pickUp;

    [Range(0, 1000)]
    public int money = 800;


    Leaf goToBackDoor;
    Leaf goToFrontDoor;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();
        Sequence steal = new Sequence("Steal Something");
        Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 1);
        Leaf goToPainting = new Leaf("Go To Painting", GoToPainting, 2);
        Leaf hasGotMoney = new Leaf("Has got Money", HasMoney);

        Leaf goToArt1 = new Leaf("Go to Art 1", GoToArt1);
        Leaf goToArt2 = new Leaf("Go to Art 2", GoToArt2);
        Leaf goToArt3 = new Leaf("Go to Art 3", GoToArt3);

        Leaf goToVan = new Leaf("Go to Van", GoToVan);
        goToBackDoor = new Leaf("Go to Backdoor", GoToBackDoor, 2);
        goToFrontDoor = new Leaf("Go to Frontdoor", GoToFrontDoor, 1);
        Selector openDoor = new Selector("Open Door");

        RSelector selectObject = new RSelector("Select Object To Steal");

        for (int i = 0; i < art.Length; i++)
        {
            //Leaf gta = new Leaf("Go to: " + art[i].name, i, GoToArt);
            Leaf gta = new Leaf("Go to: " + art[i].name, i, GoToArt);
            selectObject.AddChild(gta);
        }

        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);
        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);

        /*selectObject.AddChild(goToArt1);
        selectObject.AddChild(goToArt2);
        selectObject.AddChild(goToArt3);*/

        /*selectObject.AddChild(goToDiamond);
        selectObject.AddChild(goToPainting);*/
        steal.AddChild(selectObject);
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
        if (!diamond.activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(diamond.transform.position);

        if (s == Node.Status.SUCCES)
        {
            diamond.transform.parent = this.gameObject.transform;
            pickUp = diamond;
        }

        return s;
    }

    public Node.Status GoToArt(int i)
    {
        if (!art[i].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[i].transform.position);

        if (s == Node.Status.SUCCES)
        {
            art[i].transform.parent = this.gameObject.transform;
            pickUp = art[i];
        }

        return s;
    }

    public Node.Status GoToArt1()
    {
        if (!art[0].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[0].transform.position);

        if (s == Node.Status.SUCCES)
        {
            art[0].transform.parent = this.gameObject.transform;
            pickUp = art[0];
        }

        return s;
    }

    public Node.Status GoToArt2()
    {
        if (!art[1].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[1].transform.position);

        if (s == Node.Status.SUCCES)
        {
            art[1].transform.parent = this.gameObject.transform;
            pickUp = art[1];
        }

        return s;
    }

    public Node.Status GoToArt3()
    {
        if (!art[2].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(art[2].transform.position);

        if (s == Node.Status.SUCCES)
        {
            art[2].transform.parent = this.gameObject.transform;
            pickUp = art[2];
        }

        return s;
    }

    public Node.Status GoToPainting()
    {
        if (!painting.activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(painting.transform.position);

        if (s == Node.Status.SUCCES)
        {
            painting.transform.parent = this.gameObject.transform;
            pickUp = painting;
        }

        return s;
    }
    public Node.Status HasMoney()
    {
        if (money <= 500)
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
            pickUp.gameObject.SetActive(false);
            money += 500;
        }
        return s;
    }

    public Node.Status GoToBackDoor()
    {
        Node.Status s = GoToDoor(backDoor);
        if (s == Node.Status.FAILURE)
            goToBackDoor.sortOrder = 10;
        else
        {
            goToBackDoor.sortOrder = 1;

        }
        return s;
    }

    public Node.Status GoToFrontDoor()
    {
        Node.Status s = GoToDoor(frontDoor);
        if (s == Node.Status.FAILURE)
            goToFrontDoor.sortOrder = 10;
        else
        {
            goToFrontDoor.sortOrder = 1;

        }
        return s;
    }
    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCES)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
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
        }else if (Vector3.Distance(agent.pathEndPosition, destination) >= ConstantsNPC.DISTANCE)
        {
            state = ActionState.IDLE;
            return Node.Status.FAILURE;
        }else if  (distantToTarget  < ConstantsNPC.DISTANCE)
        {
            state = ActionState.IDLE;
            return Node.Status.SUCCES;
        }

        return Node.Status.RUNNING;

    }

}
