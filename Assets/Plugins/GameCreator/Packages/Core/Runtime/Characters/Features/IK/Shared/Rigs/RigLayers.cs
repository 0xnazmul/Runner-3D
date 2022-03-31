using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    [Serializable]
    public class RigLayers : TPolymorphicList<TRig>
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeReference] 
        protected TRig[] m_Rigs =
        {
            new RigLookTrack(),
            new RigFeetPlant(),
        };

        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Rigs.Length;

        private Character Character;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public T GetRig<T>() where T : TRig
        {
            foreach (TRig rig in this.m_Rigs) if (rig is T tRig) return tRig;
            return null;
        }
        
        // IK METHODS: ----------------------------------------------------------------------------

        public bool OnStartup(InverseKinematics inverseKinematics)
        {
            this.Character = inverseKinematics.Character;
            bool rebuildGraph = false;

            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rebuildGraph = rig.OnStartup(this.Character) || rebuildGraph;   
            }

            return rebuildGraph;
        }
        
        public bool OnEnable()
        {
            bool rebuildGraph = false;
            
            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rebuildGraph = rig.OnEnable(this.Character) || rebuildGraph;   
            }
            
            return rebuildGraph;
        }

        public bool OnDisable()
        {
            if (this.Character.Animim?.Animator == null) return false;
            bool rebuildGraph = false;

            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rebuildGraph = rig.OnDisable(this.Character) || rebuildGraph;
            }
            
            return rebuildGraph;
        }

        public bool OnUpdate()
        {
            bool rebuildGraph = false;
            
            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rebuildGraph = rig.OnUpdate(this.Character) || rebuildGraph;   
            }
            
            return rebuildGraph;
        }

        public void OnDrawGizmos()
        {
            foreach (TRig rig in this.m_Rigs)
            {
                if (!this.Character.Animim.Animator.isHuman && rig.RequiresHuman) continue;
                rig.OnDrawGizmos(this.Character);   
            }
        }
    }
}
