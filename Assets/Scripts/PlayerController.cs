using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    float thrust = 1.2f;
    Rigidbody rb;

    public IEnumerator MoveTo(GameObject agent, GameObject destination)
    {
        Debug.Log("Move to: " + destination.name);
        //agent.GetComponent<NavMeshAgent>().SetDestination(destination.transform.position);
        agent.transform.position = destination.transform.position;
        yield return new WaitForSeconds(1.0f);

    }

    public IEnumerator ReturnToStart(GameObject agent, GameObject finalWaypoint)
    {
        agent.transform.position = finalWaypoint.transform.position;
        yield return null;
    }


}
