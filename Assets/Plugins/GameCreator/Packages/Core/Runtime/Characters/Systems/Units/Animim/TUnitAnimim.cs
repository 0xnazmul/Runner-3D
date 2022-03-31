using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public abstract class TUnitAnimim : TUnit, IUnitAnimim
    {
        public const int LAYER_BREATH = 1;
        public const int LAYER_TWITCH = 2;

        protected const float LAND_RECOVERY_SMOOTH_IN = 0.4f;
        protected const float LAND_RECOVERY_DURATION = 0.1f;
        protected const float LAND_RECOVERY_SMOOTH_OUT = 0.9f;

        protected const float ROOT_MOTION_WEIGHT_SMOOTH = 0.1f;
        protected const float MODEL_OFFSET_WEIGHT_SMOOTH = 5f;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected float m_ModelPosition = 0f;
        [SerializeField] protected float m_SmoothTime = 0.5f;
        [SerializeField] protected Animator m_Animator;
        [SerializeField] protected State m_StartState;
        [SerializeField] protected BoneRack m_BoneRack = new BoneRack();

        [SerializeField] private AnimimBreathing m_Breathing = new AnimimBreathing();
        [SerializeField] private AnimimTwitching m_Twitching = new AnimimTwitching();

        // MEMBERS: -------------------------------------------------------------------------------

        private AnimimAnimatorProxy m_AnimatorProxy;
        
        private AnimFloat m_Offset = new AnimFloat(0f, MODEL_OFFSET_WEIGHT_SMOOTH);

        // PROPERTIES: ----------------------------------------------------------------------------

        public float SmoothTime
        {
            get => this.m_SmoothTime;
            set => this.m_SmoothTime = Math.Max(0f, value);
        }

        public float ModelOffset
        {
            get => this.m_Offset.Target;
            set => this.m_Offset.Target = value;
        }

        public Animator Animator
        {
            get => this.m_Animator;
            set => this.m_Animator = value;
        }

        public BoneRack BoneRack
        {
            get => this.m_BoneRack;
            set => this.m_BoneRack = value;
        }

        public Vector3 RootMotionDeltaPosition { get; private set; }
        public Quaternion RootMotionDeltaRotation { get; private set; }

        public float HeartRate
        {
            get => this.m_Breathing.Rate;
            set => this.m_Breathing.Rate = value;
        }

        public float Exertion
        {
            get => this.m_Breathing.Exertion;
            set => this.m_Breathing.Exertion = value;
        }
        
        public float Twitching
        {
            get => this.m_Twitching.Weight;
            set => this.m_Twitching.Weight = value;
        }

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<int> EventOnAnimatorIK;

        // INITIALIZATION: ------------------------------------------------------------------------
        
        public virtual void OnStartup(Character character)
        {
            this.Character = character;
            this.m_Breathing.OnStartup(this, character);
            this.m_Twitching.OnStartup(this, character);
            
            this.Character.Ragdoll.EventBeforeStartRagdoll += this.OnStartRagdoll;
            this.Character.Ragdoll.EventAfterStartRecover += this.OnEndRagdoll;
        }

        public virtual void AfterStartup(Character character)
        {
            this.Character = character;
            
            if (this.m_StartState != null)
            {
                _ = this.Character.States.SetState(
                    this.m_StartState, -1, BlendMode.Blend,
                    new ConfigState(0f, 1f, 1f, 0f, 0f)
                );
            }
        }
        
        public virtual void OnDispose(Character character)
        {
            this.Character = character;
            this.m_Breathing.OnDispose(this, character);
            this.m_Twitching.OnDispose(this, character);
            
            this.Character.Ragdoll.EventBeforeStartRagdoll -= this.OnStartRagdoll;
            this.Character.Ragdoll.EventAfterStartRecover -= this.OnEndRagdoll;
        }

        public virtual void OnEnable()
        {
            this.m_Breathing.OnEnable(this);
            this.m_Twitching.OnEnable(this);
            
            this.Character.EventLand += this.OnLand;
        }

        public virtual void OnDisable()
        {
            this.m_Breathing.OnDisable(this);
            this.m_Twitching.OnDisable(this);
            
            this.Character.EventLand -= this.OnLand;
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        public virtual void OnUpdate()
        {
            this.RequireAnimatorProxy();
            
            this.m_Breathing.OnUpdate(this);
            this.m_Twitching.OnUpdate(this);

            this.OnUpdateModelLocation();
        }

        public virtual void OnFixedUpdate()
        { }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void RequireAnimatorProxy()
        {
            if (this.m_AnimatorProxy != null) return;

            this.m_AnimatorProxy = this.Animator.gameObject.AddComponent<AnimimAnimatorProxy>();
            this.m_AnimatorProxy.Animim = this;
        }

        private void OnUpdateModelLocation()
        {
            if (this.Character.Ragdoll.IsRagdoll) return;
            
            this.m_Offset.UpdateWithDelta(this.Character.Time.DeltaTime);

            float center = -this.Character.Motion.Height * 0.5f;
            float offset = this.m_Offset.Current - this.Character.Driver.SkinWidth;
            float position = center + offset + this.m_ModelPosition;
            
            this.Animator.transform.localPosition = Vector3.up * position;
        }
        
        // CALLBACK METHODS: ----------------------------------------------------------------------

        private void OnLand(float velocity)
        {
            IUnitMotion motion = this.Character.Motion;
            float amount = Math.Abs(velocity) / (motion.JumpForce * 4f);
            
            motion.StandLevel.SetTransient(new AnimFloat.Transient(
                Mathf.Clamp01(motion.StandLevel.Current - amount),
                LAND_RECOVERY_SMOOTH_IN, 
                LAND_RECOVERY_DURATION, 
                LAND_RECOVERY_SMOOTH_OUT
            ));
        }
        
        private void OnStartRagdoll()
        {
            this.Animator.enabled = false;
        }
        
        private void OnEndRagdoll()
        {
            this.Animator.enabled = true;
        }
        
        public void OnAnimatorIK(int layerIndex)
        {
            this.EventOnAnimatorIK?.Invoke(layerIndex);
        }

        public void OnAnimatorMove()
        {
            this.Animator.applyRootMotion = true;

            this.RootMotionDeltaPosition = this.Animator.deltaPosition;
            this.RootMotionDeltaRotation = this.Animator.deltaRotation;
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        public virtual void OnDrawGizmos(Character character)
        {
            this.m_BoneRack.DrawGizmos(this.Animator);
        }
    }
}