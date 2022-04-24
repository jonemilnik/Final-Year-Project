using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Planner.Controller;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    DecisionController decisionController;
    GameObject player;
    PlayerHandler playerHandler;
    Player playerTrait;
    Mover moverTrait;
    NavMeshAgent navMAgent;
    //Used for replanning purposes
    bool enemiesLeftNearby = false;
    //Tracks which enemies need to be replanned for and
    //which have already had a replan made for
    List<GameObject> enemiesCanReplan;
    List<GameObject> walkingEnemiesCannotReplan;
    List<GameObject> inspectingEnemiesCannotReplan;
    private void Start()
    {
        decisionController = GetComponent<DecisionController>();
        player = GameObject.Find("Player");
        playerHandler = player.GetComponent<PlayerHandler>();
        playerTrait = player.GetComponent<Player>();
        moverTrait = player.GetComponent<Mover>();
        navMAgent = player.GetComponent<NavMeshAgent>();
        enemiesCanReplan = new List<GameObject>();
        walkingEnemiesCannotReplan = new List<GameObject>();
        inspectingEnemiesCannotReplan = new List<GameObject>();

    }

    private void UpdateEnemiesToReplan()
    {
        List<GameObject> nearbyEnemies = playerHandler.nearbyEnemies;

        //Add new nearby enemies to track
        for (int i = 0; i < nearbyEnemies.Count; i++)
        {
            GameObject enemy = nearbyEnemies[i];
            //Debug.Log(enemy.name);
            //If new enemy detected
            if (!enemiesCanReplan.Contains(nearbyEnemies[i]) && !walkingEnemiesCannotReplan.Contains(nearbyEnemies[i]) &&
                !inspectingEnemiesCannotReplan.Contains(nearbyEnemies[i]))
            {
                enemiesCanReplan.Add(nearbyEnemies[i]);
            }
        }

        //Remove enemies no longer nearby from both lists
        for (int i = 0; i < enemiesCanReplan.Count; i++)
        {
            GameObject enemy = enemiesCanReplan[i];
            if (!nearbyEnemies.Contains(enemy)) 
            {
                enemiesCanReplan.Remove(enemy);
                enemiesLeftNearby = true;
            }
        }

        for (int i = 0; i < walkingEnemiesCannotReplan.Count; i++)
        {
            GameObject enemy = walkingEnemiesCannotReplan[i];
            if(!nearbyEnemies.Contains(enemy))
            {
                walkingEnemiesCannotReplan.Remove(enemy);
                enemiesLeftNearby = true;
            }
        }

        for (int i = 0; i < inspectingEnemiesCannotReplan.Count; i++)
        {
            GameObject enemy = inspectingEnemiesCannotReplan[i];
            if (!nearbyEnemies.Contains(enemy))
            {
                inspectingEnemiesCannotReplan.Remove(enemy);
                enemiesLeftNearby = true;
            }
        }

        //Check if enemies that have been replanned for already have stopped and move to
        //canReplan list so a replan can be made when it moves to next waypoint
        for (int i = 0; i < walkingEnemiesCannotReplan.Count; i++)
        {
            GameObject enemy = walkingEnemiesCannotReplan[i];
            EnemyController agent = enemy.GetComponent<EnemyController>();
            if (agent.isInspecting)
            {
                enemiesCanReplan.Add(enemy);
                walkingEnemiesCannotReplan.Remove(enemy);
            }
        }

        for (int i = 0; i < inspectingEnemiesCannotReplan.Count; i++)
        {
            GameObject enemy = inspectingEnemiesCannotReplan[i];
            EnemyController agent = enemy.GetComponent<EnemyController>();
            if (agent.isWalking)
            {
                enemiesCanReplan.Add(enemy);
                inspectingEnemiesCannotReplan.Remove(enemy);
            }
        }



    }
    private void Update()
    {
        UpdateEnemiesToReplan();

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


        RePlan();
        // Update world state constantly and not just after every action
        //if (decisionController.Initialized && decisionController.IsIdle && 
        //    Time.realtimeSinceStartup > timeOfLastQueryUpdate + updateQueryDelay && !playerHandler.isSpotted)
        //{
        //    decisionController.UpdateStateWithWorldQuery();
        //    timeOfLastQueryUpdate = Time.realtimeSinceStartup;
        //}

    }

    private void RePlan()
    {

        if (decisionController.Initialized && decisionController.IsIdle)
        {
            if (enemiesLeftNearby)
            {
                decisionController.UpdateStateWithWorldQuery();
                enemiesLeftNearby = false;
            }
            
            for (int i = 0; i < enemiesCanReplan.Count; i++)
            {
                GameObject enemy = enemiesCanReplan[i];
                EnemyController agent = enemy.GetComponent<EnemyController>();
                
                if (agent.isWalking)
                {
                    walkingEnemiesCannotReplan.Add(enemy);
                    
                } else
                {
                    inspectingEnemiesCannotReplan.Add(enemy);
                }

                enemiesCanReplan.Remove(enemy);
                decisionController.UpdateStateWithWorldQuery();

            }
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
