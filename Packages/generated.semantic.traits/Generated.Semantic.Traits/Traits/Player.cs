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
    [AddComponentMenu("Semantic/Traits/Player (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Player : MonoBehaviour, ITrait
    {
        public UnityEngine.GameObject SetWaypoint
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity))
                {
                    var data = m_EntityManager.GetComponentData<PlayerData>(m_Entity);
                    if (data.SetWaypoint != default)
                        m_p0 = m_EntityManager.GetComponentObject<Transform>(data.SetWaypoint).gameObject;
                }

                return m_p0;
            }
            set
            {
                PlayerData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<PlayerData>(m_Entity);
                Entity entity = default;
                if (value != null)
                {
                    var semanticObject = value.GetComponent<SemanticObject>();
                    if (semanticObject)
                        entity = semanticObject.Entity;
                }
                m_p0 = value;
                data.SetWaypoint = entity;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Boolean IsSpotted
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity))
                {
                    m_p3 = m_EntityManager.GetComponentData<PlayerData>(m_Entity).IsSpotted;
                }

                return m_p3;
            }
            set
            {
                PlayerData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<PlayerData>(m_Entity);
                data.IsSpotted = m_p3 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Boolean IsRunning
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity))
                {
                    m_p4 = m_EntityManager.GetComponentData<PlayerData>(m_Entity).IsRunning;
                }

                return m_p4;
            }
            set
            {
                PlayerData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<PlayerData>(m_Entity);
                data.IsRunning = m_p4 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single Speed
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity))
                {
                    m_p5 = m_EntityManager.GetComponentData<PlayerData>(m_Entity).Speed;
                }

                return m_p5;
            }
            set
            {
                PlayerData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<PlayerData>(m_Entity);
                data.Speed = m_p5 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Boolean IsHiding
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity))
                {
                    m_p6 = m_EntityManager.GetComponentData<PlayerData>(m_Entity).IsHiding;
                }

                return m_p6;
            }
            set
            {
                PlayerData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<PlayerData>(m_Entity);
                data.IsHiding = m_p6 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public PlayerData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity) ?
                m_EntityManager.GetComponentData<PlayerData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<PlayerData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("SetWaypoint")]
        UnityEngine.GameObject m_p0 = default;
        [SerializeField]
        [InspectorName("IsSpotted")]
        System.Boolean m_p3 = false;
        [SerializeField]
        [InspectorName("IsRunning")]
        System.Boolean m_p4 = false;
        [SerializeField]
        [InspectorName("Speed")]
        System.Single m_p5 = 0f;
        [SerializeField]
        [InspectorName("IsHiding")]
        System.Boolean m_p6 = false;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        PlayerData GetData()
        {
            PlayerData data = default;
            if (m_p0)
            {
                var semanticObject = m_p0.GetComponent<SemanticObject>();
                if (semanticObject)
                    data.SetWaypoint = semanticObject.Entity;
            }
            data.IsSpotted = m_p3;
            data.IsRunning = m_p4;
            data.Speed = m_p5;
            data.IsHiding = m_p6;

            return data;
        }

        IEnumerator UpdateRelations()
        {
            yield return null; // Wait one frame for all game objects to be converted to entities
            SetWaypoint = m_p0;
            yield break;
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

            if (!destinationManager.HasComponent(entity, typeof(PlayerData)))
            {
                destinationManager.AddComponentData(entity, GetData());
                StartCoroutine(UpdateRelations());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<PlayerData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(PlayerData), gameObject,Data);
        }
#endif
    }
}
