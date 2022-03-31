using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Volumes")]
    
    [Serializable]
    public abstract class TVolume : TPolymorphicItem<TVolume>, IVolume
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        private Bone m_Bone = new Bone();

        [SerializeReference] 
        private IJoint m_Joint = new JointNone();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Bone {this.m_Bone} with {this.m_Joint}";
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        protected TVolume()
        { }

        protected TVolume(HumanBodyBones humanBone, IJoint joint) : this()
        {
            this.m_Bone = new Bone(humanBone);
            this.m_Joint = joint;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public GameObject UpdatePass1Physics(Animator animator, float mass, Skeleton skeleton)
        {
            Transform bone = this.m_Bone.GetTransform(animator);
            if (bone == null) return null;

            this.UpdateCollider(bone.gameObject, mass, skeleton);
            this.UpdateRigidbody(bone.gameObject, mass, skeleton);

            return bone.gameObject;
        }

        public void UpdatePass2Joints(GameObject bone, Animator animator, Skeleton skeleton)
        {
            this.m_Joint.Setup(bone, skeleton, animator);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void UpdateCollider(GameObject bone, float mass, Skeleton skeleton)
        {
            Collider collider = this.SetupCollider(bone, skeleton);
            if (collider == null) return;

            collider.material = skeleton.Material;
        }

        private void UpdateRigidbody(GameObject bone, float mass, Skeleton skeleton)
        {
            Rigidbody rigidbody = bone.Get<Rigidbody>();
            if (rigidbody == null) bone.Add<Rigidbody>();
            
            if (rigidbody == null) return;
            
            rigidbody.mass = mass / skeleton.VolumesLength;
            rigidbody.collisionDetectionMode = skeleton.CollisionDetection;
        }

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected virtual Collider SetupCollider(GameObject bone, Skeleton skeleton)
        {
            return null;
        }

        protected float GetBoneScale(Transform bone)
        {
            Vector3 scale = bone.lossyScale;
            
            if (scale.x > scale.y && scale.x > scale.z) return scale.x;
            if (scale.y > scale.x && scale.y > scale.z) return scale.y;
            return scale.z;
        }
        
        // DRAW GIZMOS: ---------------------------------------------------------------------------

        public void DrawGizmos(Animator animator, Volumes.Display display)
        {
            Transform bone = this.m_Bone?.GetTransform(animator);
            if (bone == null) return;
            
            this.DrawGizmos(bone, display);
        }
        
        protected abstract void DrawGizmos(Transform bone, Volumes.Display display);
    }
}