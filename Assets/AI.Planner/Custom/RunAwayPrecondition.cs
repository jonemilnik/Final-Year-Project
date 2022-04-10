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

        //Debug.Log("WORKING!" + GameObject.Find("Player").name);

        // EnemyController enemyController = GameObject.Find(enemyId.Name.Value).GetComponent<EnemyController>();

        //if (enemyController.GetDistToPlayer() <= 6)
        //{
        //    return true;
        //}
        //return false;
        return true;
    }
}
