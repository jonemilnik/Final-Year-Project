using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Location = Unity.AI.Planner.Traits.Location;
using Unity.AI.Planner.Traits;

public struct LeaveHidingPrecondition : ICustomActionPrecondition<StateData>
{
    public bool CheckCustomPrecondition(StateData state, ActionKey action)
    {

        //Get all queried enemies, check if facing player and distance <= 6 units
        var enemyIndices = new NativeList<int>(state.TraitBasedObjects.Length, Allocator.Temp);
        var enemyFilter = new NativeArray<ComponentType>(1, Allocator.Temp) { [0] = ComponentType.ReadWrite<Enemy>() };

        state.GetTraitBasedObjectIndices(enemyIndices, enemyFilter);
        enemyFilter.Dispose();

        //No enemies in world query
        if (enemyIndices.Length == 0)
        {
            enemyIndices.Dispose();
            return true;
        }

        for (int i = 0; i < enemyIndices.Length; i++)
        {
            var enemy = state.GetTraitOnObjectAtIndex<Enemy>(enemyIndices[i]);
            
            //If not safe for player to leave
            if (enemy.IsFacingPlayer || enemy.DistToPlayer <= 6f)
            {
                enemyIndices.Dispose();
                return false;
            }
        }

        enemyIndices.Dispose();

        //Safe to leave after enemy checks
        return true;
    }
}
