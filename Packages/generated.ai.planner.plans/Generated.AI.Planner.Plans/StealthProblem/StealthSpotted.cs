using Unity.AI.Planner;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;

namespace Generated.AI.Planner.Plans.StealthProblem
{
    public struct StealthSpotted
    {
        public bool IsTerminal(StateData stateData)
        {
            var PlayerFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Player>(),  };
            var PlayerObjectIndices = new NativeList<int>(2, Allocator.Temp);
            stateData.GetTraitBasedObjectIndices(PlayerObjectIndices, PlayerFilter);
            var PlayerBuffer = stateData.PlayerBuffer;
            for (int i0 = 0; i0 < PlayerObjectIndices.Length; i0++)
            {
                var PlayerIndex = PlayerObjectIndices[i0];
                var PlayerObject = stateData.TraitBasedObjects[PlayerIndex];
            
                
                if (!(PlayerBuffer[PlayerObject.PlayerIndex].IsSpotted == true))
                    continue;
                PlayerObjectIndices.Dispose();
                PlayerFilter.Dispose();
                return true;
            }
            PlayerObjectIndices.Dispose();
            PlayerFilter.Dispose();

            return false;
        }

        public float TerminalReward(StateData stateData)
        {
            var reward = -100f;

            return reward;
        }
    }
}
