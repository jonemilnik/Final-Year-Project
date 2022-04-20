using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Location = Unity.AI.Planner.Traits.Location;
using Unity.AI.Planner.Traits;

public struct RunAwayWaitEffect : ICustomActionEffect<StateData>
{

    public void ApplyCustomActionEffectsToState(StateData originalState, ActionKey action, StateData newState)
    {
        //calc time it will take to hideable and use to update state
        //apply state update with t = 0.5s to determine if player is spotted

        var enemyId = newState.GetTraitBasedObjectId(action[0]);
        var enemy = newState.GetTraitBasedObject(enemyId);
        var enemyTrait = newState.GetTraitOnObject<Enemy>(enemy);
        var enemyMoverTrait = newState.GetTraitOnObject<Mover>(enemy);

        var playerId = newState.GetTraitBasedObjectId(action[1]);
        var player = newState.GetTraitBasedObject(playerId);
        var playerTrait = newState.GetTraitOnObject<Player>(player);
        var playerMoverTrait = newState.GetTraitOnObject<Mover>(player);

        var hideableId = newState.GetTraitBasedObjectId(action[2]);
        var hideable = newState.GetTraitBasedObject(hideableId);
        var hideableLocationTrait = newState.GetTraitOnObject<Location>(hideable);

        //Temp vars to check if action is valid
        Vector3 enemyPos = new Vector3(enemyMoverTrait.X, enemyMoverTrait.Y, enemyMoverTrait.Z);
        Vector3 playerPos = new Vector3(playerMoverTrait.X, playerMoverTrait.Y, playerMoverTrait.Z);
        Vector3 enemyDirection = new Vector3(enemyMoverTrait.ForwardX, enemyMoverTrait.ForwardY, enemyMoverTrait.ForwardZ);

        //Varies with the different hideable args passed to action
        Vector3 playerDirection = hideableLocationTrait.Position - playerPos;

        //Calculate estimated time to reach hideable and enemy to its waypoint
        float timeToHideable = Vector3.Distance(hideableLocationTrait.Position, playerPos) / playerTrait.Speed;

        float enemyTimeToWaypoint = 0f;
        //Avoid zero division errors
        if (enemyTrait.DistToWaypoint != 0 && enemyTrait.Speed != 0)
        {
            enemyTimeToWaypoint = enemyTrait.DistToWaypoint / enemyTrait.Speed;
        }
        

        //Debug.Log("EnemyPos: " + enemyPos);
        //Debug.Log("PlayerPos: " + playerPos);
        //Debug.Log("Player Speed: " + playerTrait.Speed);
        //Debug.Log("Enemy Direction: " + enemyDirection);
        //Debug.Log("Player Direction: " + playerDirection);
        //Debug.Log("Time to hideable:" + timeToHideable);
        //Debug.Log("Enemy time to waypoint: " + enemyTimeToWaypoint);

        float timeDelta = 0f;
        //Incrementally check if player coincides with enemy's vision with t = 0.5
        while (timeDelta <= timeToHideable)
        {
            
            enemyPos += enemyDirection * 0.1f * enemyTrait.Speed;
            playerPos += playerDirection * 0.1f * playerTrait.Speed;

            //Calculate distance between enemy and player to check if player has been spotted
            float distToEnemy = Vector3.Distance(enemyPos, playerPos);
            //Player within enemy view radius
            if (distToEnemy <= enemyTrait.FOVRadius)
            {
                //Makes state undesireable to planner due to termination definition
                playerTrait.IsSpotted = true;
                break;
            }

            timeDelta += 0.1f;

            //Enemy reached its checkpoint
            if (enemyTimeToWaypoint - timeDelta <= 0)
            {
                enemyTrait.Speed = 0;
            }

        }

        //Apply necessary trait updates and add to new state

        //Update position if player not spotted
        //if (!playerTrait.IsSpotted)
        //{
        //    playerMoverTrait.X = hideableLocationTrait.Position.x;
        //    playerMoverTrait.Y = hideableLocationTrait.Position.y;
        //    playerMoverTrait.Z = hideableLocationTrait.Position.z;
        //    newState.SetTraitOnObject(playerMoverTrait, ref player);
        //}

        playerMoverTrait.X = hideableLocationTrait.Position.x;
        playerMoverTrait.Y = hideableLocationTrait.Position.y;
        playerMoverTrait.Z = hideableLocationTrait.Position.z;
        newState.SetTraitOnObject(playerMoverTrait, ref player);

        newState.SetTraitOnObject(playerTrait, ref player);




    }
}


