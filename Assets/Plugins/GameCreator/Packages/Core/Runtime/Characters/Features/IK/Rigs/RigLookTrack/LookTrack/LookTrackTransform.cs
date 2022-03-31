using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    [Serializable]
    public struct LookTrackTransform : ILookTrack
    {
        [SerializeField] private int m_Layer;
        [SerializeField] private Transform m_Transform;
        [SerializeField] private Vector3 m_Offset;
        
        // PRIORITY: ------------------------------------------------------------------------------

        public int Layer => this.m_Layer;

        public bool Exists => this.m_Transform != null;

        public Vector3 Position => this.m_Transform != null 
            ? this.m_Transform.TransformPoint(this.m_Offset)
            : default;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public LookTrackTransform(int layer, Transform transform, Vector3 offset)
        {
            this.m_Layer = layer;
            this.m_Transform = transform;
            this.m_Offset = offset;
        }
    }
}