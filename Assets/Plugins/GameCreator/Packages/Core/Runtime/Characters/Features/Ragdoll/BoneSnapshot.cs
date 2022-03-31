using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    internal class BoneSnapshot
    {
        private readonly Transform m_Reference;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Transform Value => this.m_Reference;

        public Vector3 LocalPosition { get; private set; }
        public Vector3 WorldPosition { get; private set; }

        public Quaternion LocalRotation { get; private set; }
        public Quaternion WorldRotation { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public BoneSnapshot(Transform reference)
        {
            this.m_Reference = reference;
            this.Snapshot();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Snapshot()
        {
            this.WorldPosition = this.m_Reference.position;
            this.LocalPosition = this.m_Reference.localPosition;

            this.WorldRotation = this.m_Reference.rotation;
            this.LocalRotation = this.m_Reference.localRotation;
        }
    }
}