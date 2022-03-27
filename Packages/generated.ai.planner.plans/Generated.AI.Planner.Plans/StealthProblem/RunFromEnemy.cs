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
    struct RunFromEnemy : IJobParallelForDefer
    {
        public Guid ActionGuid;
        
        const int k_EnemyIndex = 0;
        const int k_FromIndex = 1;
        const int k_ToIndex = 2;
        const int k_MaxArguments = 3;

        public static readonly string[] parameterNames = {
            "Enemy",
            "From",
            "To",
        };

        [ReadOnly] NativeArray<StateEntityKey> m_StatesToExpand;
        StateDataContext m_StateDataContext;

        // local allocations
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> EnemyFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> EnemyObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> FromFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> FromObjectIndices;
        [NativeDisableContainerSafetyRestriction] NativeArray<ComponentType> ToFilter;
        [NativeDisableContainerSafetyRestriction] NativeList<int> ToObjectIndices;

        [NativeDisableContainerSafetyRestriction] NativeList<ActionKey> ArgumentPermutations;
        [NativeDisableContainerSafetyRestriction] NativeList<RunFromEnemyFixupReference> TransitionInfo;

        bool LocalContainersInitialized => ArgumentPermutations.IsCreated;

        internal RunFromEnemy(Guid guid, NativeList<StateEntityKey> statesToExpand, StateDataContext stateDataContext)
        {
            ActionGuid = guid;
            m_StatesToExpand = statesToExpand.AsDeferredJobArray();
            m_StateDataContext = stateDataContext;
            EnemyFilter = default;
            EnemyObjectIndices = default;
            FromFilter = default;
            FromObjectIndices = default;
            ToFilter = default;
            ToObjectIndices = default;
            ArgumentPermutations = default;
            TransitionInfo = default;
        }

        void InitializeLocalContainers()
        {
            EnemyFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<Enemy>(),  };
            EnemyObjectIndices = new NativeList<int>(2, Allocator.Temp);
            FromFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<WayPoint>(),  };
            FromObjectIndices = new NativeList<int>(2, Allocator.Temp);
            ToFilter = new NativeArray<ComponentType>(1, Allocator.Temp){[0] = ComponentType.ReadWrite<WayPoint>(),  };
            ToObjectIndices = new NativeList<int>(2, Allocator.Temp);

            ArgumentPermutations = new NativeList<ActionKey>(4, Allocator.Temp);
            TransitionInfo = new NativeList<RunFromEnemyFixupReference>(ArgumentPermutations.Length, Allocator.Temp);
        }

        public static int GetIndexForParameterName(string parameterName)
        {
            
            if (string.Equals(parameterName, "Enemy", StringComparison.OrdinalIgnoreCase))
                 return k_EnemyIndex;
            if (string.Equals(parameterName, "From", StringComparison.OrdinalIgnoreCase))
                 return k_FromIndex;
            if (string.Equals(parameterName, "To", StringComparison.OrdinalIgnoreCase))
                 return k_ToIndex;

            return -1;
        }

        void GenerateArgumentPermutations(StateData stateData, NativeList<ActionKey> argumentPermutations)
        {
            EnemyObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(EnemyObjectIndices, EnemyFilter);
            
            FromObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(FromObjectIndices, FromFilter);
            
            ToObjectIndices.Clear();
            stateData.GetTraitBasedObjectIndices(ToObjectIndices, ToFilter);
            
            
            

            for (int i0 = 0; i0 < EnemyObjectIndices.Length; i0++)
            {
                var EnemyIndex = EnemyObjectIndices[i0];
                var EnemyObject = stateData.TraitBasedObjects[EnemyIndex];
                
                
                
            
            

            for (int i1 = 0; i1 < FromObjectIndices.Length; i1++)
            {
                var FromIndex = FromObjectIndices[i1];
                var FromObject = stateData.TraitBasedObjects[FromIndex];
                
                
                
            
            

            for (int i2 = 0; i2 < ToObjectIndices.Length; i2++)
            {
                var ToIndex = ToObjectIndices[i2];
                var ToObject = stateData.TraitBasedObjects[ToIndex];
                
                
                

                var actionKey = new ActionKey(k_MaxArguments) {
                                                        ActionGuid = ActionGuid,
                                                       [k_EnemyIndex] = EnemyIndex,
                                                       [k_FromIndex] = FromIndex,
                                                       [k_ToIndex] = ToIndex,
                                                    };
                  if (!new global::AI.Planner.Custom.StealthProblem.RunAwayPrecondition().CheckCustomPrecondition(stateData, actionKey))
                    continue;

                argumentPermutations.Add(actionKey);
            
            }
            
            }
            
            }
        }

        StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> ApplyEffects(ActionKey action, StateEntityKey originalStateEntityKey)
        {
            var originalState = m_StateDataContext.GetStateData(originalStateEntityKey);
            var originalStateObjectBuffer = originalState.TraitBasedObjects;
            var originalFromObject = originalStateObjectBuffer[action[k_FromIndex]];

            var newState = m_StateDataContext.CopyStateData(originalState);
            var newWayPointBuffer = newState.WayPointBuffer;
            {
                    var @WayPoint = newWayPointBuffer[originalFromObject.WayPointIndex];
                    @WayPoint.Left = newWayPointBuffer[originalFromObject.WayPointIndex].Left;
                    newWayPointBuffer[originalFromObject.WayPointIndex] = @WayPoint;
            }

            

            var reward = Reward(originalState, action, newState);
            var StateTransitionInfo = new StateTransitionInfo { Probability = 1f, TransitionUtilityValue = reward };
            var resultingStateKey = m_StateDataContext.GetStateDataKey(newState);

            return new StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>(originalStateEntityKey, action, resultingStateKey, StateTransitionInfo);
        }

        float Reward(StateData originalState, ActionKey action, StateData newState)
        {
            var reward = -100f;

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
                TransitionInfo.Add(new RunFromEnemyFixupReference { TransitionInfo = ApplyEffects(ArgumentPermutations[i], stateEntityKey) });
            }

            // fixups
            var stateEntity = stateEntityKey.Entity;
            var fixupBuffer = m_StateDataContext.EntityCommandBuffer.AddBuffer<RunFromEnemyFixupReference>(jobIndex, stateEntity);
            fixupBuffer.CopyFrom(TransitionInfo);
        }

        
        public static T GetEnemyTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_EnemyIndex]);
        }
        
        public static T GetFromTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_FromIndex]);
        }
        
        public static T GetToTrait<T>(StateData state, ActionKey action) where T : struct, ITrait
        {
            return state.GetTraitOnObjectAtIndex<T>(action[k_ToIndex]);
        }
        
    }

    public struct RunFromEnemyFixupReference : IBufferElementData
    {
        internal StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo> TransitionInfo;
    }
}


