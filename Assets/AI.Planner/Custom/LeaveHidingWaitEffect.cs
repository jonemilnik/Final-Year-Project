using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Location = Unity.AI.Planner.Traits.Location;
using Unity.AI.Planner.Traits;

public struct LeaveHidingWaitEffect : ICustomActionEffect<StateData>
{
    public void ApplyCustomActionEffectsToState(StateData originalState, ActionKey action, StateData newState)
    {
        //newState.GetTraitBasedObjectIndices()
    }
}
