using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Location = Unity.AI.Planner.Traits.Location;
using Unity.AI.Planner.Traits;

//float waitTime = 0.5f;

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
        Vector3 playerDirection = new Vector3(playerMoverTrait.ForwardX, playerMoverTrait.ForwardY, playerMoverTrait.ForwardZ);

        //Calculate estimated time to reach hideable 
        float timeToHideable = Vector3.Distance(hideableLocationTrait.Position, playerPos) / playerTrait.Speed;

        float timeDelta = 0f;

        while (timeDelta <= timeToHideable)
        {
            enemyPos += enemyDirection * 0.5f * enemyTrait.Speed;
            playerPos += playerDirection * 0.5f * playerTrait.Speed;

            //Calculate distance between enemy and player to check if player has been spotted
            float distToEnemy = Vector3.Distance(enemyPos, playerPos);


            



        }

    }
}


