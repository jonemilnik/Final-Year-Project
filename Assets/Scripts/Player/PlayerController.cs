using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Planner.Controller;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    float updateQueryDelay = 0.5f;
    float timeOfLastQueryUpdate;
    DecisionController decisionController;
    GameObject player;
    PlayerHandler playerHandler;
    Player playerTrait;
    Mover moverTrait;
    NavMeshAgent navMAgent;
 

    private void Start()
    {
        decisionController = GetComponent<DecisionController>();
        player = GameObject.Find("Player");
        playerHandler = player.GetComponent<PlayerHandler>();
        playerTrait = player.GetComponent<Player>();
        moverTrait = player.GetComponent<Mover>();
        navMAgent = player.GetComponent<NavMeshAgent>();
        //navMAgent = player.GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        playerTrait.IsSpotted = player.GetComponent<PlayerHandler>().isSpotted;
        moverTrait.X = player.transform.position.x;
        moverTrait.Y = player.transform.position.y;
        moverTrait.Z = player.transform.position.z;

        moverTrait.ForwardX = player.transform.forward.x;
        moverTrait.ForwardY = player.transform.forward.y;
        moverTrait.ForwardZ = player.transform.forward.z;
        //Debug.Log("Player running: " + playerTrait.IsRunning);

        // Update world state constantly and not just after every action
        if (decisionController.Initialized && decisionController.IsIdle && 
            Time.realtimeSinceStartup > timeOfLastQueryUpdate + updateQueryDelay && !playerHandler.isSpotted)
        {
            decisionController.UpdateStateWithWorldQuery();
            timeOfLastQueryUpdate = Time.realtimeSinceStartup;
        }

    }

    public IEnumerator MoveTo(GameObject destination)
    {
        navMAgent.SetDestination(destination.transform.position);
        //while (true)
        //{
        //    if (!navMAgent.pathPending)
        //    {
        //        if (navMAgent.remainingDistance <= navMAgent.stoppingDistance)
        //        {
        //            if (!navMAgent.hasPath || navMAgent.velocity.sqrMagnitude == 0f)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    yield return null;
        //}

        //playerTrait.SetWaypoint = destination;

        yield return null;
    }

    public IEnumerator RunAway(GameObject destination)
    {
        //Debug.Log("Running away to: " + closestWaypoint.name);
        navMAgent.SetDestination(destination.transform.position);
        playerHandler.isRunning = true;
        //while (true)
        //{
        //    if (!navMAgent.pathPending)
        //    {
        //        if (navMAgent.remainingDistance <= navMAgent.stoppingDistance)
        //        {
        //            if (!navMAgent.hasPath || navMAgent.velocity.sqrMagnitude == 0f)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    yield return null;
        //}
        yield return null;
        
    }

    public IEnumerator Wait()
    {
        playerHandler.isHiding = true;
        playerHandler.isRunning = false;
        yield return new WaitForSeconds(1.5f);
    }





}
