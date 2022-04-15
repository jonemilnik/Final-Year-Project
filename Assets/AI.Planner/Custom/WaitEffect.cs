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
        var enemyLocationTrait = originalState.GetTraitOnObject<Location>(enemy);
        var enemyTrait = originalState.GetTraitOnObject<Enemy>(enemy);

        var playerId = originalState.GetTraitBasedObjectId(action[1]);
        var player = originalState.GetTraitBasedObject(playerId);
        var playerLocationTrait = originalState.GetTraitOnObject<Location>(player);
        var playerTrait = originalState.GetTraitOnObject<Player>(player);

        var hideableId = originalState.GetTraitBasedObjectId(action[2]);
        var hideable = originalState.GetTraitBasedObject(hideableId);
        var hideableLocationTrait = originalState.GetTraitOnObject<Location>(hideable);

        //Calculate distance between enemy and player to check if player has been spotted
        float distToEnemy = Vector3.Distance(enemyLocationTrait.Position, playerLocationTrait.Position);

        //Calculate distance between player and hideable spot 
        float distToHideable = Vector3.Distance(hideableLocationTrait.Position, playerLocationTrait.Position);




    }
}


