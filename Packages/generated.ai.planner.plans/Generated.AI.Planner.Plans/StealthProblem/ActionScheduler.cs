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
        public static readonly Guid MoveDownGuid = Guid.NewGuid();
        public static readonly Guid MoveLeftGuid = Guid.NewGuid();
        public static readonly Guid MoveRightGuid = Guid.NewGuid();
        public static readonly Guid MoveUpGuid = Guid.NewGuid();
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
            public EntityCommandBuffer MoveDownECB;
            public EntityCommandBuffer MoveLeftECB;
            public EntityCommandBuffer MoveRightECB;
            public EntityCommandBuffer MoveUpECB;
            public EntityCommandBuffer RunAwayECB;

            public void Execute()
            {
                // Playback entity changes and output state transition info
                var entityManager = ExclusiveEntityTransaction;

                MoveDownECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveDownRefs = entityManager.GetBuffer<MoveDownFixupReference>(stateEntity);
                    for (int j = 0; j < MoveDownRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveDownRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveDownFixupReference));
                }

                MoveLeftECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveLeftRefs = entityManager.GetBuffer<MoveLeftFixupReference>(stateEntity);
                    for (int j = 0; j < MoveLeftRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveLeftRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveLeftFixupReference));
                }

                MoveRightECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveRightRefs = entityManager.GetBuffer<MoveRightFixupReference>(stateEntity);
                    for (int j = 0; j < MoveRightRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveRightRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveRightFixupReference));
                }

                MoveUpECB.Playback(entityManager);
                for (int i = 0; i < UnexpandedStates.Length; i++)
                {
                    var stateEntity = UnexpandedStates[i].Entity;
                    var MoveUpRefs = entityManager.GetBuffer<MoveUpFixupReference>(stateEntity);
                    for (int j = 0; j < MoveUpRefs.Length; j++)
                        CreatedStateInfo.Enqueue(MoveUpRefs[j].TransitionInfo);
                    entityManager.RemoveComponent(stateEntity, typeof(MoveUpFixupReference));
                }

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
            var MoveDownDataContext = StateManager.StateDataContext;
            var MoveDownECB = StateManager.GetEntityCommandBuffer();
            MoveDownDataContext.EntityCommandBuffer = MoveDownECB.AsParallelWriter();
            var MoveLeftDataContext = StateManager.StateDataContext;
            var MoveLeftECB = StateManager.GetEntityCommandBuffer();
            MoveLeftDataContext.EntityCommandBuffer = MoveLeftECB.AsParallelWriter();
            var MoveRightDataContext = StateManager.StateDataContext;
            var MoveRightECB = StateManager.GetEntityCommandBuffer();
            MoveRightDataContext.EntityCommandBuffer = MoveRightECB.AsParallelWriter();
            var MoveUpDataContext = StateManager.StateDataContext;
            var MoveUpECB = StateManager.GetEntityCommandBuffer();
            MoveUpDataContext.EntityCommandBuffer = MoveUpECB.AsParallelWriter();
            var RunAwayDataContext = StateManager.StateDataContext;
            var RunAwayECB = StateManager.GetEntityCommandBuffer();
            RunAwayDataContext.EntityCommandBuffer = RunAwayECB.AsParallelWriter();

            var allActionJobs = new NativeArray<JobHandle>(6, Allocator.TempJob)
            {
                [0] = new MoveDown(MoveDownGuid, UnexpandedStates, MoveDownDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [1] = new MoveLeft(MoveLeftGuid, UnexpandedStates, MoveLeftDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [2] = new MoveRight(MoveRightGuid, UnexpandedStates, MoveRightDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [3] = new MoveUp(MoveUpGuid, UnexpandedStates, MoveUpDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [4] = new RunAway(RunAwayGuid, UnexpandedStates, RunAwayDataContext).Schedule(UnexpandedStates, 0, inputDeps),
                [5] = entityManager.ExclusiveEntityTransactionDependency
            };

            var allActionJobsHandle = JobHandle.CombineDependencies(allActionJobs);
            allActionJobs.Dispose();

            // Playback entity changes and output state transition info
            var playbackJob = new PlaybackECB()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                UnexpandedStates = UnexpandedStates,
                CreatedStateInfo = m_CreatedStateInfo,
                MoveDownECB = MoveDownECB,
                MoveLeftECB = MoveLeftECB,
                MoveRightECB = MoveRightECB,
                MoveUpECB = MoveUpECB,
                RunAwayECB = RunAwayECB,
            };

            var playbackJobHandle = playbackJob.Schedule(allActionJobsHandle);
            entityManager.ExclusiveEntityTransactionDependency = playbackJobHandle;

            return playbackJobHandle;
        }
    }
}
