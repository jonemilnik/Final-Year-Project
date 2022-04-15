using Unity.AI.Planner;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;

namespace Generated.AI.Planner.Plans.StealthProblem
{
    public struct StealthGoal
    {
        public bool IsTerminal(StateData stateData)
        {
            var GoalLocationFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<GoalPoint>(),[1] = ComponentType.ReadWrite<Location>(),  };
            var GoalLocationObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(GoalLocationObjectIndices, GoalLocationFilter);
            var PlayerFilter = new NativeArray<ComponentType>(2, Allocator.Temp){[0] = ComponentType.ReadWrite<Player>(),[1] = ComponentType.ReadWrite<Mover>(),  };
            var PlayerObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(PlayerObjectIndices, PlayerFilter);
            var PlayerBuffer = stateData.PlayerBuffer;
            for (int i0 = 0; i0 < GoalLocationObjectIndices.Length; i0++)
            {
                var GoalLocationIndex = GoalLocationObjectIndices[i0];
                var GoalLocationObject = stateData.TraitBasedObjects[GoalLocationIndex];
            
                
            for (int i1 = 0; i1 < PlayerObjectIndices.Length; i1++)
            {
                var PlayerIndex = PlayerObjectIndices[i1];
                var PlayerObject = stateData.TraitBasedObjects[PlayerIndex];
            
                
                if (!(PlayerBuffer[PlayerObject.PlayerIndex].SetWaypoint == stateData.GetTraitBasedObjectId(GoalLocationIndex)))
                    continue;
                GoalLocationObjectIndices.Dispose();
                GoalLocationFilter.Dispose();
                PlayerObjectIndices.Dispose();
                PlayerFilter.Dispose();
                return true;
            }
            }
            GoalLocationObjectIndices.Dispose();
            GoalLocationFilter.Dispose();
            PlayerObjectIndices.Dispose();
            PlayerFilter.Dispose();

            return false;
        }

        public float TerminalReward(StateData stateData)
        {
            var reward = 100f;

            return reward;
        }
    }
}
