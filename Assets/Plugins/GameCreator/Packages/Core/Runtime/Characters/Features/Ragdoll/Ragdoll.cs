using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class Ragdoll
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] protected float m_TransitionDuration = 0.75f;
        [SerializeField] protected AnimationClip m_RecoverFaceDown;
        [SerializeField] protected AnimationClip m_RecoverFaceUp;

        // MEMBERS: -------------------------------------------------------------------------------

        private Character m_Character;

        private GameObject[] m_Bones = new GameObject[0];
        private int m_CurrentModelID = -1;

        private BoneSnapshot m_RootSnapshot;
        private BoneSnapshot[] m_BonesSnapshots;

        private bool m_IsRecovering;
        private float m_RecoverStartTime;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public bool IsRagdoll { get; private set; }

        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action EventBeforeStartRagdoll;
        public event Action EventAfterStartRagdoll;
        
        public event Action EventBeforeStartRecover;
        public event Action EventAfterStartRecover;
        
        // INITIALIZE METHODS: --------------------------------------------------------------------
        
        internal void OnStartup(Character character)
        {
            this.m_Character = character;

            if (this.m_Character.Animim.BoneRack == null) return;
            this.m_Character.Animim.BoneRack.EventChangeSkeleton += this.OnChangeSkeleton;
        }
        
        internal void AfterStartup(Character character)
        { }

        internal void OnDispose(Character character)
        {
            this.m_Character = character;
            
            if (this.m_Character.Animim.BoneRack == null) return;
            this.m_Character.Animim.BoneRack.EventChangeSkeleton -= this.OnChangeSkeleton;
        }

        internal void OnEnable()
        { }

        internal void OnDisable()
        { }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        internal void OnLateUpdate()
        {
            if (!this.IsRagdoll) return;
            
            Animator animator = this.m_Character.Animim.Animator;
            if (animator == null) return;

            if (!this.m_IsRecovering) return;
            this.UpdateRagdollRecover(animator);
        }
        
        // UPDATE METHODS: ------------------------------------------------------------------------

        private void UpdateRagdollRecover(Animator animator)
        {
            float elapsed = this.m_Character.Time.Time - this.m_RecoverStartTime;
            float t = Easing.QuadInOut(0f, 1f, elapsed / this.m_TransitionDuration);
        
            this.m_RootSnapshot.Value.localPosition = Vector3.Lerp(
                this.m_RootSnapshot.LocalPosition,
                this.m_RootSnapshot.Value.localPosition,
                t
            );
            
            this.m_RootSnapshot.Value.localRotation = Quaternion.Lerp(
                this.m_RootSnapshot.LocalRotation,
                this.m_RootSnapshot.Value.localRotation,
                t
            );
            
            foreach (BoneSnapshot boneSnapshot in this.m_BonesSnapshots)
            {
                if (boneSnapshot.Value == null) continue;
                
                if (boneSnapshot.Value.parent == animator.transform)
                {
                    boneSnapshot.Value.position = Vector3.Lerp(
                        boneSnapshot.WorldPosition,
                        boneSnapshot.Value.position,
                        t
                    );
                }
        
                if (boneSnapshot.LocalRotation != boneSnapshot.Value.localRotation)
                {
                    boneSnapshot.Value.rotation = Quaternion.Lerp(
                        boneSnapshot.WorldRotation,
                        boneSnapshot.Value.rotation,
                        t
                    );
                }
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public async Task StartRagdoll()
        {
            if (this.IsRagdoll) return;
            if (this.m_Character.Animim.Animator == null) return;

            this.m_IsRecovering = false;

            this.RequireInitialize(false);
            
            Vector3 direction = this.m_Character.Driver.WorldMoveDirection;
            
            this.EventBeforeStartRagdoll?.Invoke();
            
            this.m_Character.Busy.SetBusy();
            this.m_Character.Gestures.Stop(0f, 0f);
            
            await Task.Yield();
            
            this.m_Character.Animim.Animator.transform.SetParent(null);

            foreach (GameObject bone in this.m_Bones)
            {
                bone.Get<Collider>().enabled = true;
                bone.Get<Rigidbody>().isKinematic = false;
                bone.Get<Rigidbody>().velocity = direction;
            }

            this.IsRagdoll = true;
            
            this.EventAfterStartRagdoll?.Invoke();
        }

        public async Task StartRecover()
        {
            if (!this.IsRagdoll) return;
            if (this.m_Character.Animim.Animator == null) return;

            Animator animator = this.m_Character.Animim.Animator;
            this.EventBeforeStartRecover?.Invoke();
            
            this.RequireInitialize(false);

            foreach (GameObject bone in this.m_Bones)
            {
                bone.Get<Collider>().enabled = false;
                bone.Get<Rigidbody>().isKinematic = true;
                bone.Get<Rigidbody>().velocity = Vector3.zero;
            }

            Transform model = animator.transform;
            if (animator.isHuman)
            {
                Transform hips = animator.GetBoneTransform(HumanBodyBones.Hips);
                Transform head = animator.GetBoneTransform(HumanBodyBones.Head);

                Ray ray = new Ray(
                    hips.position, 
                    Vector3.down * (this.m_Character.Motion.Height * 0.5f)
                );
                
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 pointDifference = model.position - hit.point;
                    model.position -= pointDifference;
                    hips.position += pointDifference;
                }
                else
                {
                    Vector3 hipsDifference = model.position - hips.position;
                    model.position -= hipsDifference;
                    hips.position += hipsDifference;
                }
                
                Vector3 skeletonForward = Vector3.ProjectOnPlane(
                    head.position - hips.position, 
                    Vector3.up
                );

                Quaternion hipsRotation = hips.rotation;
                Vector3 hipsForward = hips.TransformDirection(Vector3.forward);
                
                model.rotation = (
                    Quaternion.LookRotation(skeletonForward, Vector3.up) *
                    Quaternion.Euler(0f, hipsForward.y > 0f ? 180f : 0f, 0f)
                );
                
                hips.rotation = hipsRotation;
            }

            this.m_Character.Driver.SetPosition(
                model.position + 
                Vector3.up * (this.m_Character.Motion.Height * 0.5f)
            );
            
            this.m_Character.Driver.SetRotation(model.rotation);
            
            model.SetParent(this.m_Character.transform);
            model.localPosition = Vector3.down * (this.m_Character.Motion.Height * 0.5f);
            model.localRotation = Quaternion.identity;
            
            this.EventAfterStartRecover?.Invoke();

            AnimationClip standClip;
            if (animator.isHuman)
            {
                Vector3 hipsForward = animator
                    .GetBoneTransform(HumanBodyBones.Hips)
                    .TransformDirection(Vector3.forward);

                standClip = hipsForward.y > 0f
                    ? this.m_RecoverFaceUp
                    : this.m_RecoverFaceDown;
            }
            else
            {
                standClip = model.TransformDirection(Vector3.forward).y > 0f
                    ? this.m_RecoverFaceUp
                    : this.m_RecoverFaceDown;
            }
            
            if (standClip != null)
            {
                const BlendMode mode = BlendMode.Blend;
                ConfigGesture config = new ConfigGesture(
                    0f, standClip.length, 1f, true,
                    0f, this.m_Character.Animim.SmoothTime
                );
            
                this.m_IsRecovering = true;
                this.m_RecoverStartTime = this.m_Character.Time.Time;
                this.RefreshSnapshots();
            
                await this.m_Character.Gestures.CrossFade(standClip, null, mode, config, true);
            }
            
            this.m_Character.Busy.SetAvailable();
            this.IsRagdoll = false;
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RequireInitialize(bool force)
        {
            if (this.m_Character == null) return;
            if (this.m_Character.Animim.Animator == null) return;

            Skeleton skeleton = this.m_Character.Animim?.BoneRack.Skeleton;
            if (skeleton == null) return;

            int modelID = this.m_Character.Animim.Animator.gameObject.GetInstanceID();
            if (modelID == this.m_CurrentModelID && !force) return;

            this.m_Bones = skeleton.Refresh(this.m_Character);
            this.m_CurrentModelID = modelID;
        }
        
        private void RefreshSnapshots()
        {
            Animator animator = this.m_Character.Animim.Animator;
            if (animator == null) return;

            Transform[] children = animator.GetComponentsInChildren<Transform>();
            
            this.m_RootSnapshot = new BoneSnapshot(animator.transform);
            List<BoneSnapshot> candidates = new List<BoneSnapshot>();
            
            foreach (Transform child in children)
            {
                if (child == animator.transform) continue;
                candidates.Add(new BoneSnapshot(child));
            }
            
            this.m_BonesSnapshots = candidates.ToArray();
        }

        // CALLBACKS: -----------------------------------------------------------------------------

        private void OnChangeSkeleton()
        {
            this.m_CurrentModelID = -1;
        }
    }
}