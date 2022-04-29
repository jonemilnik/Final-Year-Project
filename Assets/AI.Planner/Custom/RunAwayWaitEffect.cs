using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Location = Unity.AI.Planner.Traits.Location;
using Unity.AI.Planner.Traits;

public struct RunAwayWaitEffect : ICustomActionEffect<StateData>
{

    // Propagates enemy changes in the state incrementally with t = 0.25 
    // Also checks to see if player is spotted and sets the trait property to true
    public void ApplyCustomActionEffectsToState(StateData originalState, ActionKey action, StateData newState)
    {

        var playerId = newState.GetTraitBasedObjectId(action[1]);
        var player = newState.GetTraitBasedObject(playerId);
        var playerTrait = newState.GetTraitOnObject<Player>(player);
        var playerMoverTrait = newState.GetTraitOnObject<Mover>(player);

        var hideableId = newState.GetTraitBasedObjectId(action[2]);
        var hideable = newState.GetTraitBasedObject(hideableId);
        var hideableLocationTrait = newState.GetTraitOnObject<Location>(hideable);

        //Get all queried enemies as enemyIndices
        var enemyIndices = new NativeList<int>(newState.TraitBasedObjects.Length, Allocator.Temp);
        var enemyFilter = new NativeArray<ComponentType>(1, Allocator.Temp) { [0] = ComponentType.ReadWrite<Enemy>() };

        newState.GetTraitBasedObjectIndices(enemyIndices, enemyFilter);
        enemyFilter.Dispose();

        

        //Iterate through each enemy to see whether player gets spotted by one when moving to hideable
        for (int i = 0; i < enemyIndices.Length; i++)
        {
            var enemyTrait = newState.GetTraitOnObjectAtIndex<Enemy>(enemyIndices[i]);
            var enemyMoverTrait = newState.GetTraitOnObjectAtIndex<Mover>(enemyIndices[i]);

            //Temp vars to check if action is valid
            Vector3 enemyPos = new Vector3(enemyMoverTrait.X, enemyMoverTrait.Y, enemyMoverTrait.Z);
            Vector3 playerPos = new Vector3(playerMoverTrait.X, playerMoverTrait.Y, playerMoverTrait.Z);
            float enemySpeed = enemyTrait.Speed;
            float playerSpeed = playerTrait.Speed;

            //Calculate enemy direction to waypoint
            Vector3 enemyNextWaypoint = new Vector3(enemyTrait.WaypointX, enemyTrait.WaypointY, enemyTrait.WaypointZ);
            Vector3 eTemp = (-enemyPos + enemyNextWaypoint).normalized;
            Vector3 enemyDirection = new Vector3(eTemp.x, 0, eTemp.z);

            //Calculate player direction to waypoint
            Vector3 pTemp = (hideableLocationTrait.Position - playerPos).normalized;
            Vector3 playerDirection = new Vector3(pTemp.x, 0, pTemp.z);

            //Calculate estimated time to reach hideable and enemy to its waypoint
            float timeToHideable = Vector3.Distance(hideableLocationTrait.Position, playerPos) / playerTrait.Speed;

            float enemyTimeToWaypoint = 0f;
            //Avoid zero division errors
            if (enemyTrait.DistToWaypoint != 0 && enemyTrait.Speed != 0)
            {
                enemyTimeToWaypoint = enemyTrait.DistToWaypoint / enemyTrait.Speed;
            }

            float timeDelta = 0f;
            //Incrementally check if player coincides with enemy's vision with t = 0.25
            while (timeDelta <= timeToHideable)
            {

                enemyPos = enemyPos + (enemyDirection * 0.25f * enemySpeed);
                playerPos = playerPos + (playerDirection * 0.25f * playerSpeed);

                //Calculate euclidean distance between enemy and player to check if player has been spotted
                float distToEnemy = Vector3.Distance(enemyPos, playerPos);

                //Player within enemy view radius
                if (distToEnemy <= enemyTrait.FOVRadius)
                {
                    //Makes state undesireable to planner due to termination definition
                    playerTrait.IsSpotted = true;
                    break;
                }

                timeDelta += 0.25f;

                //Enemy reached its checkpoint
                if (enemyTimeToWaypoint - timeDelta <= 0)
                {
                    enemySpeed = 0;
                }

            }
        }

        //Apply necessary trait updates and add to new state
        playerMoverTrait.X = hideableLocationTrait.Position.x;
        playerMoverTrait.Y = hideableLocationTrait.Position.y;
        playerMoverTrait.Z = hideableLocationTrait.Position.z;
        newState.SetTraitOnObject(playerMoverTrait, ref player);

        newState.SetTraitOnObject(playerTrait, ref player);

        enemyIndices.Dispose();



    }
}


