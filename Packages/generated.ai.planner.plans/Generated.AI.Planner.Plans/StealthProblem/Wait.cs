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
    struct Wait : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_EnemyIndex = 0;
        const int k_AgentIndex = 1;
        const int k_HideableIndex = 2;
        const int k_MaxArguments = 3;

        public static readonly string[] parameterNames = {
            "Enemy",
            "Agent",
            "Hideable",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> EnemyFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> EnemyObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> AgentFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> AgentObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> HideableFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> HideableObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<WaitFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal Wait(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            EnemyFilter = default;
            EnemyObjectIndices = default;
            AgentFilter = default;
            AgentObjectIndices = default;
            HideableFilter = default;
            HideableObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            EnemyFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Enemy>(),  };
            EnemyObjectIndices = new NativeList<int>(2, Allocator.Temp);
            AgentFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Player>(),  };
            AgentObjectIndices = new NativeList<int>(2, Allocator.Temp);
            HideableFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Hideable>(),  };
            HideableObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<WaitFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Enemy", StringComparison.OrdinalIgnoreCase))
                 return k_EnemyIndex;
            if (string.Equals(parameterName, "Agent", StringComparison.OrdinalIgnoreCase))
                 return k_AgentIndex;
            if (string.Equals(parameterName, "Hideable", StringComparison.OrdinalIgnoreCase))
                 return k_HideableIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            EnemyObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(EnemyObjectIndices, EnemyFilter);
            
            AgentObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(AgentObjectIndices, AgentFilter);
            
            HideableObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(HideableObjectIndices, HideableFilter);
            
            var PlayerBuffer = stateData.PlayerBuffer;
            
            

            for (int i0 = 0; i0 < EnemyObjectIndices.Length; i0++)
            {
                var EnemyIndex = EnemyObjectIndices[i0];
                var EnemyObject = stateData.TraitBasedObjects[EnemyIndex];
                
                
                
                
                
            
            

            for (int i1 = 0; i1 < AgentObjectIndices.Length; i1++)
            {
                var AgentIndex = AgentObjectIndices[i1];
                var AgentObject = stateData.TraitBasedObjects[AgentIndex];
                
                
                if (!(PlayerBuffer[AgentObject.PlayerIndex].IsRunning == true))
                    continue;
                
                
                
            
            

            for (int i2 = 0; i2 < HideableObjectIndices.Length; i2++)
            {
                var HideableIndex = HideableObjectIndices[i2];
                var HideableObject = stateData.TraitBasedObjects[HideableIndex];
                
                if (!(PlayerBuffer[AgentObject.PlayerIndex].SetWaypoint == stateData.GetTraitBasedObjectId(HideableIndex)))
                    continue;
                
                
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_EnemyIndex] = EnemyIndex,
                                                       [k_AgentIndex] = AgentIndex,
                                                       [k_HideableIndex] = HideableIndex,
                                                    };
                argumentPermutations.Add(actionKey);
            
            }
            
            }
            
            }
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;

            var newState = m_StateDataContext.CopyStateData(originalState);
            {
                    new global::WaitEffect().ApplyCustomActionEffectsToState(originalState, action, newState);
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = 0f;

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
                TransitionInfo.Add(new WaitFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<WaitFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetEnemyTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_EnemyIndex]);
        }
        
        public static T GetAgentTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_AgentIndex]);
        }
        
        public static T GetHideableTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_HideableIndex]);
        }
        
    }

    public struct WaitFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


