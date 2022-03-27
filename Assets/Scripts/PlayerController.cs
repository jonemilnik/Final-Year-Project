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
    float updateQueryDelay = 0.25f;
    float timeOfLastQueryUpdate;
    DecisionController decisionController;
    Player playerTrait;
 

    private void Start()
    {
        decisionController = GetComponent<DecisionController>();
        playerTrait = GameObject.Find("Player").GetComponent<Player>();
       
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
        NavMeshAgent navMAgent = agent.GetComponent<NavMeshAgent>();
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

        Player playerTrait = agent.GetComponent<Player>();
        playerTrait.Waypoint = destination;
    }

    



}
