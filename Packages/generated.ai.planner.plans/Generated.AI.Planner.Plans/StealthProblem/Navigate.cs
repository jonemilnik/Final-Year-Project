using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.Burst;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Generated.AI.Planner.Plans.StealthProblem
{
    [BurstCompile]
    struct Navigate : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_ToIndex = 0;
        const int k_AgentIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "To",
            "Agent",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> ToFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> ToObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> AgentFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> AgentObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<NavigateFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal Navigate(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            ToFilter = default;
            ToObjectIndices = default;
            AgentFilter = default;
            AgentObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            ToFilter = new NativeArray<ComponentType>(3, Allocator.Temp){[0] = ComponentType.ReadWrite<GoalPoint>(),[1] = ComponentType.ReadWrite<Location>(), [2] = ComponentType.Exclude<Hideable>(),  };
            ToObjectIndices = new NativeList<int>(2, Allocator.Temp);
            AgentFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Player>(),  };
            AgentObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<NavigateFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "To", StringComparison.OrdinalIgnoreCase))
                 return k_ToIndex;
            if (string.Equals(parameterName, "Agent", StringComparison.OrdinalIgnoreCase))
                 return k_AgentIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            ToObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(ToObjectIndices, ToFilter);
            
            AgentObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(AgentObjectIndices, AgentFilter);
            
            
            

            for (int i0 = 0; i0 < ToObjectIndices.Length; i0++)
            {
                var ToIndex = ToObjectIndices[i0];
                var ToObject = stateData.TraitBasedObjects[ToIndex];
                
                
            
            

            for (int i1 = 0; i1 < AgentObjectIndices.Length; i1++)
            {
                var AgentIndex = AgentObjectIndices[i1];
                var AgentObject = stateData.TraitBasedObjects[AgentIndex];
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_ToIndex] = ToIndex,
                                                       [k_AgentIndex] = AgentIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            }
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalAgentObject = originalStateObjectBuffer[action[k_AgentIndex]];
            var originalToObject = originalStateObjectBuffer[action[k_ToIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newPlayerBuffer = newState.PlayerBuffer;
            {
                    var @Player = newPlayerBuffer[originalAgentObject.PlayerIndex];
                    @Player.@SetWaypoint = originalState.GetTraitBasedObjectId(originalToObject);
                    newPlayerBuffer[originalAgentObject.PlayerIndex] = @Player;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = -1f;

            return reward;
        }

        public void Execute(int jobIndex)
        {
            if (!LocalContainersInitialized)
                InitializeLocalContainers();

            m_StateDataContext.JobIndex = jobIndex;

            var stateEntityKey = m_StatesToExpand[jobIndex];
            var stateData = m_StateDataContext.GetStateData(stateEntityKey);

            ArgumentPermutations.Clear();
            GenerateArgumentPermutations(stateData, ArgumentPermutations);

            TransitionInfo.Clear();
            TransitionInfo.Capacity = math.max(TransitionInfo.Capacity, ArgumentPermutations.Length);
            for (var i = 0; i < ArgumentPermutations.Length; i++)
            {
                TransitionInfo.Add(new NavigateFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<NavigateFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetToTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_ToIndex]);
        }
        
        public static T GetAgentTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_AgentIndex]);
        }
        
    }

    public struct NavigateFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


