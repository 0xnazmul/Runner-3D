using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters.IK
{
    [Serializable]
    public struct LookTrackPosition : ILookTrack
    {
        [SerializeField] private int m_Layer;
        [SerializeField] private Vector3 m_Position;
        
        // PRIORITY: ------------------------------------------------------------------------------

        public int Layer => this.m_Layer;
        
        public bool Exists => true;
        
        public Vector3 Position => this.m_Position;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public LookTrackPosition(int layer, Vector3 position)
        {
            this.m_Layer = layer;
            this.m_Position = position;
        }
    }
}