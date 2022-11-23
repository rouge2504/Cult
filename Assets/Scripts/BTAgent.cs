using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
    public BehaviourTree tree;

    public NavMeshAgent agent;

    public enum ActionState { IDLE, WORKING};
    public ActionState state = ActionState.IDLE;

    public Node.Status treeStatus = Node.Status.RUNNING;

    WaitForSeconds waitForSeconds;

    // Start is called before the first frame update
    public void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        tree = new BehaviourTree();

        waitForSeconds = new WaitForSeconds(Random.Range(0.1f, 1));
        StartCoroutine(Behave());
    }

    public Node.Status GoToLocation(Vector3 destination)
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

    IEnumerator Behave()
    {
        while (true)
        {
            treeStatus = tree.Procces();
            yield return waitForSeconds;
        }
    }

}
