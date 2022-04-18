using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Location = Unity.AI.Planner.Traits.Location;
using Unity.AI.Planner.Traits;

public struct WaitEffect : ICustomActionEffect<StateData>
{

    public void ApplyCustomActionEffectsToState(StateData originalState, ActionKey action, StateData newState)
    {
        //calc time it will take to hideable and use to update state
        //apply state update with t = 0.5s to determine if player is spotted

        var enemyId = originalState.GetTraitBasedObjectId(action[0]);
        var enemy = originalState.GetTraitBasedObject(enemyId);
        var enemyTrait = originalState.GetTraitOnObject<Enemy>(enemy);
        var enemyMoverTrait = originalState.GetTraitOnObject<Mover>(enemy);

        var playerId = originalState.GetTraitBasedObjectId(action[1]);
        var player = originalState.GetTraitBasedObject(playerId);
        var playerTrait = originalState.GetTraitOnObject<Player>(player);
        var playerMoverTrait = originalState.GetTraitOnObject<Mover>(player);

        var hideableId = originalState.GetTraitBasedObjectId(action[2]);
        var hideable = originalState.GetTraitBasedObject(hideableId);
        var hideableLocationTrait = originalState.GetTraitOnObject<Location>(hideable);

        //Temp vars to check if action is valid
        Vector3 enemyPos = new Vector3(enemyMoverTrait.X, enemyMoverTrait.Y, enemyMoverTrait.Z);
        Vector3 playerPos = new Vector3(playerMoverTrait.X, playerMoverTrait.Y, playerMoverTrait.Z);
        Vector3 enemyDirection = new Vector3(enemyMoverTrait.ForwardX, enemyMoverTrait.ForwardY, enemyMoverTrait.ForwardZ);

        //Varies with the different hideable args passed to action
        Vector3 playerDirection = hideableLocationTrait.Position - playerPos;

        //Calculate estimated time to reach hideable and enemy to its waypoint
        float timeToHideable = Vector3.Distance(hideableLocationTrait.Position, playerPos) / playerTrait.Speed;
        float enemyTimeToWaypoint = enemyTrait.DistToWaypoint / enemyTrait.Speed;

        //Debug.Log("EnemyPos: " + enemyPos);
        //Debug.Log("PlayerPos: " + playerPos);
        //Debug.Log("Player Speed: " + playerTrait.Speed);
        //Debug.Log("Enemy Direction: " + enemyDirection);
        //Debug.Log("Player Direction: " + playerDirection);
        //Debug.Log("Time to hideable:" + timeToHideable);
        //Debug.Log("Enemy time to waypoint: " + enemyTimeToWaypoint);

        float timeDelta = 0f;
        int iterations = 0;
        //Incrementally check if player coincides with enemy's vision with t = 0.5
        while (timeDelta <= timeToHideable)
        {
            
            enemyPos += enemyDirection * 0.5f * enemyTrait.Speed;
            playerPos += playerDirection * 0.5f * playerTrait.Speed;

            //Calculate distance between enemy and player to check if player has been spotted
            float distToEnemy = Vector3.Distance(enemyPos, playerPos);

            //Player within enemy view radius
            if (distToEnemy <= enemyTrait.FOVRadius)
            {
                //Makes state undesireable to planner due to termination definition
                playerTrait.IsSpotted = true;
                //Debug.Log(string.Format("Spottable Location: ", hideableId.Name.ToString()) );
                break;
            }

            timeDelta += 0.5f;

            //Enemy reached its checkpoint
            if (enemyTimeToWaypoint - timeDelta <= 0)
            {
                enemyTrait.Speed = 0;
            }

        }

        //Apply necessary trait updates (mover trait details are redundant as they will be updated with world state when subplan is complete)
        playerTrait.IsRunning = false;
       

        //Add to new state
        newState.SetTraitOnObject(playerTrait, ref player);



    }
}


