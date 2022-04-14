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
        public static readonly Guid RunAwayGuid = Guid.NewGuid();

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
            public EntityCommandBuffer RunAwayECB;

            public void Execute()
            {
                // Playback entity changes and output state transition info
                var entityManager = ExclusiveEntityTransaction;

                RunAwayECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var RunAwayRefs = entityManager.GetBuffer<RunAwayFixupReference>(stateEntity);
                    for (int j = 0; j < RunAwayRefs.Length; j++)
                        CreatedStateInfo.Enqueue(RunAwayRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(RunAwayFixupReference));
                }
            }
        }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var entityManager = StateManager.ExclusiveEntityTransaction.EntityManager;
            var RunAwayDataContext = StateManager.StateDataContext;
            var RunAwayECB = StateManager.GetEntityCommandBuffer();
            RunAwayDataContext.EntityCommandBuffer = RunAwayECB.AsParallelWriter();

            var allActionJobs = new NativeArray<JobHandle>(2, Allocator.TempJob)
            {
                [0] = new RunAway(RunAwayGuid, UnexpandedStates, RunAwayDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [1] = entityManager.ExclusiveEntityTransactionDependency
            };

            var allActionJobsHandle = JobHandle.CombineDependencies(allActionJobs);
            allActionJobs.Dispose();

            // Playback entity changes and output state transition info
            var playbackJob = new PlaybackECB()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                UnexpandedStates = UnexpandedStates,
                CreatedStateInfo = m_CreatedStateInfo,
                RunAwayECB = RunAwayECB,
            };

            var playbackJobHandle = playbackJob.Schedule(allActionJobsHandle);
            entityManager.ExclusiveEntityTransactionDependency = playbackJobHandle;

            return playbackJobHandle;
        }
    }
}
