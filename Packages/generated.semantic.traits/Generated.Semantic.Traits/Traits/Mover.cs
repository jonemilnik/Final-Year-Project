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
    [AddComponentMenu("Semantic/Traits/Mover (Trait)")]
    [RequireComponent(typeof(SemanticObject))]
    public partial class Mover : MonoBehaviour, ITrait
    {
        public System.Single X
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity))
                {
                    m_p0 = m_EntityManager.GetComponentData<MoverData>(m_Entity).X;
                }

                return m_p0;
            }
            set
            {
                MoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<MoverData>(m_Entity);
                data.X = m_p0 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single Y
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity))
                {
                    m_p1 = m_EntityManager.GetComponentData<MoverData>(m_Entity).Y;
                }

                return m_p1;
            }
            set
            {
                MoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<MoverData>(m_Entity);
                data.Y = m_p1 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single Z
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity))
                {
                    m_p2 = m_EntityManager.GetComponentData<MoverData>(m_Entity).Z;
                }

                return m_p2;
            }
            set
            {
                MoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<MoverData>(m_Entity);
                data.Z = m_p2 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single ForwardX
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity))
                {
                    m_p3 = m_EntityManager.GetComponentData<MoverData>(m_Entity).ForwardX;
                }

                return m_p3;
            }
            set
            {
                MoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<MoverData>(m_Entity);
                data.ForwardX = m_p3 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single ForwardY
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity))
                {
                    m_p4 = m_EntityManager.GetComponentData<MoverData>(m_Entity).ForwardY;
                }

                return m_p4;
            }
            set
            {
                MoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<MoverData>(m_Entity);
                data.ForwardY = m_p4 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public System.Single ForwardZ
        {
            get
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity))
                {
                    m_p5 = m_EntityManager.GetComponentData<MoverData>(m_Entity).ForwardZ;
                }

                return m_p5;
            }
            set
            {
                MoverData data = default;
                var dataActive = m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity);
                if (dataActive)
                    data = m_EntityManager.GetComponentData<MoverData>(m_Entity);
                data.ForwardZ = m_p5 = value;
                if (dataActive)
                    m_EntityManager.SetComponentData(m_Entity, data);
            }
        }
        public MoverData Data
        {
            get => m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity) ?
                m_EntityManager.GetComponentData<MoverData>(m_Entity) : GetData();
            set
            {
                if (m_EntityManager != default && m_EntityManager.HasComponent<MoverData>(m_Entity))
                    m_EntityManager.SetComponentData(m_Entity, value);
            }
        }

        #pragma warning disable 649
        [SerializeField]
        [InspectorName("X")]
        System.Single m_p0 = 0f;
        [SerializeField]
        [InspectorName("Y")]
        System.Single m_p1 = 0f;
        [SerializeField]
        [InspectorName("Z")]
        System.Single m_p2 = 0f;
        [SerializeField]
        [InspectorName("ForwardX")]
        System.Single m_p3 = 0f;
        [SerializeField]
        [InspectorName("ForwardY")]
        System.Single m_p4 = 0f;
        [SerializeField]
        [InspectorName("ForwardZ")]
        System.Single m_p5 = 0f;
        #pragma warning restore 649

        EntityManager m_EntityManager;
        World m_World;
        Entity m_Entity;

        MoverData GetData()
        {
            MoverData data = default;
            data.X = m_p0;
            data.Y = m_p1;
            data.Z = m_p2;
            data.ForwardX = m_p3;
            data.ForwardY = m_p4;
            data.ForwardZ = m_p5;

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

            if (!destinationManager.HasComponent(entity, typeof(MoverData)))
            {
                destinationManager.AddComponentData(entity, GetData());
            }
        }

        void OnDestroy()
        {
            if (m_World != default && m_World.IsCreated)
            {
                m_EntityManager.RemoveComponent<MoverData>(m_Entity);
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
            TraitGizmos.DrawGizmoForTrait(nameof(MoverData), gameObject,Data);
        }
#endif
    }
}
