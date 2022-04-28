using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Unity.AI.Planner.Traits;

public struct RunAwayPrecondition : ICustomActionPrecondition<StateData>
{
    public bool CheckCustomPrecondition(StateData state, ActionKey action)
    {
        
        var enemyId = state.GetTraitBasedObjectId(action[0]);
        var enemy = state.GetTraitBasedObject(enemyId);
        var enemyTrait = state.GetTraitOnObject<Enemy>(enemy);

        //If enemy within radius of 5 units or facing player
        if (enemyTrait.IsFacingPlayer || enemyTrait.DistToPlayer <= enemyTrait.FOVRadius + 2f)
        {
            return true;
        }

        return false;
    }
}
