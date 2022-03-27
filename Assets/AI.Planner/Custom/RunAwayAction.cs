using System.Collections;
using System.Collections.Generic;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using UnityEngine;
using Unity.AI.Planner.Traits;




namespace AI.Planner.Custom.StealthProblem
{
    public struct RunAwayPrecondition : ICustomActionPrecondition<StateData>

    {
        public bool CheckCustomPrecondition(StateData state, ActionKey action)
        {
            Debug.Log("State Data: " + state.ToString());
            Debug.Log("Action: " + action.ToString());
            
            //Check if enemy facing player and within certain radius

            
            return true;
        }
    }
}

