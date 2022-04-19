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
    struct Hide : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_HideableIndex = 0;
        const int k_AgentIndex = 1;
        const int k_MaxArguments = 2;

        public static readonly string[] parameterNames = {
            "Hideable",
            "Agent",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> HideableFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> HideableObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> AgentFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> AgentObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<HideFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal Hide(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            HideableFilter = default;
            HideableObjectIndices = default;
            AgentFilter = default;
            AgentObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            HideableFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Hideable>(),  };
            HideableObjectIndices = new NativeList<int>(2, Allocator.Temp);
            AgentFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Player>(),  };
            AgentObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<HideFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Hideable", StringComparison.OrdinalIgnoreCase))
                 return k_HideableIndex;
            if (string.Equals(parameterName, "Agent", StringComparison.OrdinalIgnoreCase))
                 return k_AgentIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            HideableObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(HideableObjectIndices, HideableFilter);
            
            AgentObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(AgentObjectIndices, AgentFilter);
            
            var PlayerBuffer = stateData.PlayerBuffer;
            
            

            for (int i0 = 0; i0 < HideableObjectIndices.Length; i0++)
            {
                var HideableIndex = HideableObjectIndices[i0];
                var HideableObject = stateData.TraitBasedObjects[HideableIndex];
                
                
                
            
            

            for (int i1 = 0; i1 < AgentObjectIndices.Length; i1++)
            {
                var AgentIndex = AgentObjectIndices[i1];
                var AgentObject = stateData.TraitBasedObjects[AgentIndex];
                
                if (!(PlayerBuffer[AgentObject.PlayerIndex].SetWaypoint == stateData.GetTraitBasedObjectId(HideableIndex)))
                    continue;
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_HideableIndex] = HideableIndex,
                                                       [k_AgentIndex] = AgentIndex,
                                                    };
                  if (!new global::HidePrecondition().CheckCustomPrecondition(stateData, actionKey))
                    continue;

                argumentPermutations.Add(actionKey);
            
            }
            
            }
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalAgentObject = originalStateObjectBuffer[action[k_AgentIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newPlayerBuffer = newState.PlayerBuffer;
            {
                    var @Player = newPlayerBuffer[originalAgentObject.PlayerIndex];
                    @Player.@IsHiding = true;
                    newPlayerBuffer[originalAgentObject.PlayerIndex] = @Player;
            }
            {
                    var @Player = newPlayerBuffer[originalAgentObject.PlayerIndex];
                    @Player.@IsRunning = false;
                    newPlayerBuffer[originalAgentObject.PlayerIndex] = @Player;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = 1f;

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
                TransitionInfo.Add(new HideFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<HideFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetHideableTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_HideableIndex]);
        }
        
        public static T GetAgentTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_AgentIndex]);
        }
        
    }

    public struct HideFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


