using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class Volumes : TPolymorphicList<TVolume>
    {
        public enum Display
        {
            None,
            Outline,
            Solid
        }
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeReference] private IVolume[] m_Volumes;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Volumes.Length;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Volumes()
        {
            this.m_Volumes = Array.Empty<IVolume>();
        }

        public Volumes(params IVolume[] volumes)
        {
            List<IVolume> candidates = new List<IVolume>();
            foreach (IVolume volume in volumes)
            {
                if (volume == null) continue;
                candidates.Add(volume);
            }
            
            this.m_Volumes = candidates.ToArray();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject[] Update(Animator animator, float mass, Skeleton skeleton)
        {
            GameObject[] targets = new GameObject[this.m_Volumes.Length];
            for (int i = 0; i < this.m_Volumes.Length; ++i)
            {
                targets[i] = this.m_Volumes[i].UpdatePass1Physics(animator, mass, skeleton);
            }
            
            for (int i = 0; i < this.m_Volumes.Length; ++i)
            {
                if (targets[i] == null) continue;
                this.m_Volumes[i].UpdatePass2Joints(targets[i], animator, skeleton);
            }

            return targets;
        }

        // DRAW GIZMOS: ---------------------------------------------------------------------------

        public void DrawGizmos(Animator animator, Volumes.Display display)
        {
            Gizmos.color = new Color(256f, 0f, 0f, 0.5f);
            foreach (IVolume volume in this.m_Volumes)
            {
                volume.DrawGizmos(animator, display);
            }
        }
    }
}