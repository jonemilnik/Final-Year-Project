using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Planner.Controller;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    float speed = 1.2f;
    float updateQueryDelay = 0.05f;
    float timeOfLastQueryUpdate;
    DecisionController decisionController;
    GameObject player;
    Player playerTrait;
    NavMeshAgent navMAgent;
 

    private void Start()
    {
        decisionController = GetComponent<DecisionController>();
        player = GameObject.Find("Player");
        playerTrait = player.GetComponent<Player>();
        navMAgent = player.GetComponent<NavMeshAgent>();
        //navMAgent = player.GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        playerTrait.IsSpotted = GameObject.Find("Player").GetComponent<PlayerHandler>().isSpotted;

        // Update world state constantly and not just after every action
        if (decisionController.Initialized && Time.realtimeSinceStartup > timeOfLastQueryUpdate + updateQueryDelay)
        {
            decisionController.UpdateStateWithWorldQuery();
            timeOfLastQueryUpdate = Time.realtimeSinceStartup;
        }

    }

    public IEnumerator MoveTo(GameObject agent, GameObject destination)
    {
        navMAgent.SetDestination(destination.transform.position);
        while (true)
        {
            if (!navMAgent.pathPending)
            {
                if (navMAgent.remainingDistance <= navMAgent.stoppingDistance)
                {
                    if (!navMAgent.hasPath || navMAgent.velocity.sqrMagnitude == 0f)
                    {
                        break;
                    }
                }
            }
            yield return null;
        }

        playerTrait.Waypoint = destination;

        //yield return null;
    }

    public IEnumerator RunAway(GameObject agent)
    {
        
        GameObject waypointsObj = GameObject.Find("PlayerWaypoints");
        GameObject closestWaypoint = null;
        float closestDist = Mathf.Infinity;

        // Find waypoint with closest distance to player
        for (int i = 0; i < waypointsObj.transform.childCount; i++)
        {
            GameObject waypoint = waypointsObj.transform.GetChild(i).gameObject;
            //Debug.Log("Waypoint: " + waypoint.name);
            float dist = (waypoint.transform.position - player.transform.position).sqrMagnitude;
            //Debug.Log("Waypoint Dist: " + dist);
            
            if (dist < closestDist)
            {
                closestDist = dist;
                closestWaypoint = waypoint;
                Debug.Log("PlayerTrait Waypoint: " + playerTrait.Waypoint.name);
                Debug.Log("Closest waypoint: " + closestWaypoint.name);
            }

            
        }

        //Debug.Log("Running away to: " + closestWaypoint.name);
        navMAgent.SetDestination(closestWaypoint.transform.position);
        while (true)
        {
            if (!navMAgent.pathPending)
            {
                if (navMAgent.remainingDistance <= navMAgent.stoppingDistance)
                {
                    if (!navMAgent.hasPath || navMAgent.velocity.sqrMagnitude == 0f)
                    {
                        break;
                    }
                }
            }
            yield return null;
        }


        playerTrait.Waypoint = closestWaypoint;
        
        
    }





}
