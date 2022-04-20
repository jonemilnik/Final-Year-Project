using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Location = Unity.AI.Planner.Traits.Location;
using Unity.AI.Planner.Traits;

public struct RunAwayReward : ICustomActionReward<StateData>
{
    public float RewardModifier(StateData originalState, ActionKey action, StateData newState)
    {
        var playerId = originalState.GetTraitBasedObjectId(action[1]);
        var player = originalState.GetTraitBasedObject(playerId);
        var playerMoverTrait = originalState.GetTraitOnObject<Mover>(player);

        var hideableId = originalState.GetTraitBasedObjectId(action[2]);
        var hideable = originalState.GetTraitBasedObject(hideableId);
        var hideableLocationTrait = originalState.GetTraitOnObject<Location>(hideable);

        Vector3 playerPos = new Vector3(playerMoverTrait.X, playerMoverTrait.Y, playerMoverTrait.Z);
        Vector3 hideablePos = hideableLocationTrait.Position;

        float distance = Vector3.Distance(hideablePos, playerPos);

        return distance;

    }
}

