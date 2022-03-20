using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Planner.Controller;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits;


public class PlayerController : MonoBehaviour
{
    float thrust = 1.2f;
    float updateStateDelay = 0.25f;
     

    //private void Update()
    //{
        
    //}

    public IEnumerator MoveTo(GameObject agent, GameObject destination)
    {
        Debug.Log("Move to: " + destination.name);
        //agent.GetComponent<NavMeshAgent>().SetDestination(destination.transform.position);
        agent.transform.position = destination.transform.position;
        //ITrait trait= agent.GetComponent<ITrait>();
        //Component[] components = agent.GetComponents<Component>();
        //foreach (Component component in components)
        //{
        //    Debug.Log(component.ToString());
        //}
        Player trait = agent.GetComponent<Player>();
        trait.Waypoint = destination;
        //Debug.Log("Player components: " + GameObject.Find("Player").GetComponents<Component>());
        yield return new WaitForSeconds(1.0f);

    }

    public IEnumerator ReturnToStart(GameObject agent, GameObject finalWaypoint)
    {
        agent.transform.position = finalWaypoint.transform.position;
        yield return null;
    }


}
