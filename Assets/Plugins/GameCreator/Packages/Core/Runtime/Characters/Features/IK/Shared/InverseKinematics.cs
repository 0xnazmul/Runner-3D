using System;
using GameCreator.Runtime.Characters.IK;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class InverseKinematics
    {
        // MEMBERS: -------------------------------------------------------------------------------

        private Character m_Character;
        
        private GameObject m_Model;
        private RigBuilder m_RigBuilder;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private RigLayers m_RigLayers = new RigLayers();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Character Character => this.m_Character;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventRigBuilderCreate;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject Model
        {
            get
            {
                if (this.m_Model == null)
                {
                    this.m_Model = this.m_Character.Animim.Animator.gameObject;
                }

                return this.m_Model;
            }
        }

        public RigBuilder RigBuilder
        {
            get
            {
                if (this.m_RigBuilder == null)
                {
                    this.m_RigBuilder = this.Model.GetComponent<RigBuilder>();
                    this.EventRigBuilderCreate?.Invoke();
                }
            
                if (this.m_RigBuilder == null)
                {
                    this.m_RigBuilder = this.Model.AddComponent<RigBuilder>();
                    this.EventRigBuilderCreate?.Invoke();
                }

                return this.m_RigBuilder;
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public T GetRig<T>() where T : TRig
        {
            return this.m_RigLayers.GetRig<T>();
        }
        
        public bool HasRig<T>() where T : TRig
        {
            return this.m_RigLayers.GetRig<T>() != null;
        }
        
        // LIFECYCLE METHODS: ---------------------------------------------------------------------
        
        internal void OnStartup(Character character)
        {
            this.m_Character = character;
            if (this.m_RigLayers.OnStartup(this)) this.RebuildRig();
        }
        
        internal void AfterStartup(Character character)
        { }

        internal void OnEnable()
        {
            if (this.m_RigLayers.OnEnable()) this.RebuildRig();
        }

        internal void OnDisable()
        {
            if (this.m_RigLayers.OnDisable()) this.RebuildRig();
        }
        
        internal void OnUpdate()
        {
            if (this.m_RigLayers.OnUpdate()) this.RebuildRig();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RebuildRig()
        {
            this.RigBuilder.Build();
        }

        // GIZMOS: --------------------------------------------------------------------------------
        
        public void OnDrawGizmos(Character character)
        {
            if (!Application.isPlaying) return;
            this.m_RigLayers.OnDrawGizmos();
        }
    }
}