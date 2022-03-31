using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    internal class Prop
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly IBone m_Bone;

        private readonly Vector3 m_OffsetPosition;
        private readonly Quaternion m_OffsetRotation;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GameObject Prefab { get; }
        
        public Transform InstanceBone { get; private set; }
        public GameObject InstancePrefab { get; private set; }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public Prop(IBone bone, GameObject prefab, Vector3 position, Quaternion rotation)
        {
            this.m_Bone = bone;
            this.Prefab = prefab;
            
            this.m_OffsetPosition = position;
            this.m_OffsetRotation = rotation;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void Create(Animator animator)
        {
            if (animator == null) return;
            if (this.Prefab == null) return;
            
            this.InstanceBone = this.m_Bone?.GetTransform(animator);
            if (this.InstanceBone == null) return;

            this.InstancePrefab = Object.Instantiate(this.Prefab);

            this.InstancePrefab.transform.localScale = Vector3.one;
            this.InstancePrefab.transform.SetParent(this.InstanceBone, true);
            
            this.InstancePrefab.transform.localPosition = this.m_OffsetPosition;
            this.InstancePrefab.transform.localRotation = this.m_OffsetRotation;
        }

        public void Destroy()
        {
            if (this.InstancePrefab == null) return;
            Object.Destroy(this.InstancePrefab);
        }
    }
}