using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Planner.Controller;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    float updateQueryDelay = 0.1f;
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
        playerTrait.IsSpotted = playerHandler.isSpotted;
        playerTrait.IsHiding = playerHandler.isHiding;
        playerTrait.IsRunning = playerHandler.isRunning;
        moverTrait.X = player.transform.position.x;
        moverTrait.Y = player.transform.position.y;
        moverTrait.Z = player.transform.position.z;

        moverTrait.ForwardX = player.transform.forward.x;
        moverTrait.ForwardY = player.transform.forward.y;
        moverTrait.ForwardZ = player.transform.forward.z;
        //Debug.Log("Agent speed: " + navMAgent.velocity.magnitude);

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

        playerTrait.SetWaypoint = destination;
        playerTrait.IsWaypointSet = true;

        yield return new WaitForSeconds(.5f);
    }

    public IEnumerator RunAway(GameObject destination)
    {
        navMAgent.SetDestination(destination.transform.position);
        playerTrait.SetWaypoint = destination;
        playerTrait.IsWaypointSet = true;
        
        yield return new WaitForSeconds(.5f);
        
    }

    public IEnumerator Hide()
    {
        playerHandler.Hide();
        playerTrait.IsWaypointSet = false;
        yield return new WaitForSeconds(.5f);
        
    }

    public IEnumerator LeaveHiding()
    {
        playerHandler.StopHiding();
        playerTrait.IsWaypointSet = false;
        yield return new WaitForSeconds(.5f);
    }





}
