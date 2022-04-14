using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Semantic.Traits;
using Unity.Entities;
using UnityEngine;

namespace Generated.Semantic.Traits
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Semantic/Traits/Enemy (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Enemy : MonoBehaviour, ITrait
    {
        public System.Boolean IsFacingPlayer
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity))
                {
                    m_p2 = m_EntityManager.GetComponentData<EnemyData>(m_Entity).IsFacingPlayer;
                }

                return m_p2;
            }
            set
            {
                EnemyData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<EnemyData>(m_Entity);
                data.IsFacingPlayer = m_p2 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single DistToPlayer
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity))
                {
                    m_p3 = m_EntityManager.GetComponentData<EnemyData>(m_Entity).DistToPlayer;
                }

                return m_p3;
            }
            set
            {
                EnemyData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<EnemyData>(m_Entity);
                data.DistToPlayer = m_p3 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single Speed
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity))
                {
                    m_p5 = m_EntityManager.GetComponentData<EnemyData>(m_Entity).Speed;
                }

                return m_p5;
            }
            set
            {
                EnemyData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<EnemyData>(m_Entity);
                data.Speed = m_p5 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single DistToWaypoint
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity))
                {
                    m_p6 = m_EntityManager.GetComponentData<EnemyData>(m_Entity).DistToWaypoint;
                }

                return m_p6;
            }
            set
            {
                EnemyData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<EnemyData>(m_Entity);
                data.DistToWaypoint = m_p6 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single FOVRadius
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity))
                {
                    m_p8 = m_EntityManager.GetComponentData<EnemyData>(m_Entity).FOVRadius;
                }

                return m_p8;
            }
            set
            {
                EnemyData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<EnemyData>(m_Entity);
                data.FOVRadius = m_p8 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public EnemyData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity) ?
                m_EntityManager.GetComponentData<EnemyData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<EnemyData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("IsFacingPlayer")]
        System.Boolean m_p2 = false;
        [SerializeField]
        [InspectorName("DistToPlayer")]
        System.Single m_p3 = 0f;
        [SerializeField]
        [InspectorName("Speed")]
        System.Single m_p5 = 3.5f;
        [SerializeField]
        [InspectorName("DistToWaypoint")]
        System.Single m_p6 = 0f;
        [SerializeField]
        [InspectorName("FOVRadius")]
        System.Single m_p8 = 4f;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        EnemyData GetData()
        {
            EnemyData data = default;
            data.IsFacingPlayer = m_p2;
            data.DistToPlayer = m_p3;
            data.Speed = m_p5;
            data.DistToWaypoint = m_p6;
            data.FOVRadius = m_p8;

            return data;
        }

        
        void OnEnable()
        {
            // Handle the case where this trait is added after conversion
            var semanticObject = GetComponent<SemanticObject>();
            if (semanticObject && !semanticObject.Entity.Equals(default))
                Convert(semanticObject.Entity, semanticObject.EntityManager, null);
        }

        public void Convert(Entity entity, EntityManager destinationManager, GameObjectConversionSystem _)
        {
            m_Entity = entity;
            m_EntityManager = destinationManager;
            m_World = destinationManager.World;

            if (!destinationManager.HasComponent(entity, typeof(EnemyData)))
            {
                destinationManager.AddComponentData(entity, GetData());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<EnemyData>(m_Entity);
                if (m_EntityManager.GetComponentCount(m_Entity) == 0)
                    m_EntityManager.DestroyEntity(m_Entity);
            }
        }

        void OnValidate()
        {

            // Commit local fields to backing store
            Data = GetData();
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            TraitGizmos.DrawGizmoForTrait(nameof(EnemyData), gameObject,Data);
        }
#endif
    }
}
