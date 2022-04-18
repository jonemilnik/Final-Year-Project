using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Unity.AI.Planner.Traits;
using Location = Unity.AI.Planner.Traits.Location;

public struct HidePrecondition : ICustomActionPrecondition<StateData>
{
    public bool CheckCustomPrecondition(StateData state, ActionKey action)
    {
        var hideableId = state.GetTraitBasedObjectId(action[0]);
        var hideable = state.GetTraitBasedObject(hideableId);
        var hideableLocationTrait = state.GetTraitOnObject<Location>(hideable);

        var playerId = state.GetTraitBasedObjectId(action[1]);
        var player = state.GetTraitBasedObject(playerId);
        var playerTrait = state.GetTraitOnObject<Player>(player);
        var playerMoverTrait = state.GetTraitOnObject<Mover>(player);

        Vector3 playerPos = new Vector3(playerMoverTrait.X, playerMoverTrait.Y, playerMoverTrait.Z);

        //Check player is within 1 unit from hideable location waypoint on path
        if ( Vector3.Distance(playerPos, hideableLocationTrait.Position) <= 1 )
        {
            return true;
        }

        return false;

    }
}
