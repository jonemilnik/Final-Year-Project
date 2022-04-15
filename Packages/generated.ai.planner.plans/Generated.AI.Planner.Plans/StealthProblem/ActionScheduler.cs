using System;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.AI.Planner.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Generated.AI.Planner.StateRepresentation;
using Generated.AI.Planner.StateRepresentation.StealthProblem;

namespace Generated.AI.Planner.Plans.StealthProblem
{
    public struct ActionScheduler :
        ITraitBasedActionScheduler<TraitBasedObject, StateEntityKey, StateData, StateDataContext, StateManager, ActionKey>
    {
        public static readonly Guid RunAwayStartGuid = Guid.NewGuid();
        public static readonly Guid NavigateGuid = Guid.NewGuid();

        // Input
        public NativeList<StateEntityKey> UnexpandedStates { get; set; }
        public StateManager StateManager { get; set; }

        // Output
        NativeQueue<StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>> IActionScheduler<StateEntityKey, StateData, StateDataContext, StateManager, ActionKey>.CreatedStateInfo
        {
            set => m_CreatedStateInfo = value;
        }

        NativeQueue<StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>> m_CreatedStateInfo;

        struct PlaybackECB : IJob
        {
            public ExclusiveEntityTransaction ExclusiveEntityTransaction;

            [ReadOnly]
            public NativeList<StateEntityKey> UnexpandedStates;
            public NativeQueue<StateTransitionInfoPair<StateEntityKey, ActionKey, StateTransitionInfo>> CreatedStateInfo;
            public EntityCommandBuffer RunAwayStartECB;
            public EntityCommandBuffer NavigateECB;

            public void Execute()
            {
                // Playback entity changes and output state transition info
                var entityManager = ExclusiveEntityTransaction;

                RunAwayStartECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var RunAwayStartRefs = entityManager.GetBuffer<RunAwayStartFixupReference>(stateEntity);
                    for (int j = 0; j < RunAwayStartRefs.Length; j++)
                        CreatedStateInfo.Enqueue(RunAwayStartRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(RunAwayStartFixupReference));
                }

                NavigateECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var NavigateRefs = entityManager.GetBuffer<NavigateFixupReference>(stateEntity);
                    for (int j = 0; j < NavigateRefs.Length; j++)
                        CreatedStateInfo.Enqueue(NavigateRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(NavigateFixupReference));
                }
            }
        }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var entityManager = StateManager.ExclusiveEntityTransaction.EntityManager;
            var RunAwayStartDataContext = StateManager.StateDataContext;
            var RunAwayStartECB = StateManager.GetEntityCommandBuffer();
            RunAwayStartDataContext.EntityCommandBuffer = RunAwayStartECB.AsParallelWriter();
            var NavigateDataContext = StateManager.StateDataContext;
            var NavigateECB = StateManager.GetEntityCommandBuffer();
            NavigateDataContext.EntityCommandBuffer = NavigateECB.AsParallelWriter();

            var allActionJobs = new NativeArray<JobHandle>(3, Allocator.TempJob)
            {
                [0] = new RunAwayStart(RunAwayStartGuid, UnexpandedStates, RunAwayStartDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [1] = new Navigate(NavigateGuid, UnexpandedStates, NavigateDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [2] = entityManager.ExclusiveEntityTransactionDependency
            };

            var allActionJobsHandle = JobHandle.CombineDependencies(allActionJobs);
            allActionJobs.Dispose();

            // Playback entity changes and output state transition info
            var playbackJob = new PlaybackECB()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                UnexpandedStates = UnexpandedStates,
                CreatedStateInfo = m_CreatedStateInfo,
                RunAwayStartECB = RunAwayStartECB,
                NavigateECB = NavigateECB,
            };

            var playbackJobHandle = playbackJob.Schedule(allActionJobsHandle);
            entityManager.ExclusiveEntityTransactionDependency = playbackJobHandle;

            return playbackJobHandle;
        }
    }
}
