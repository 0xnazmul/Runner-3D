using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [AddComponentMenu("")]
    public abstract class TCamera : MonoBehaviour
    {
        private enum RunMode
        {
            MainUpdate,
            FixedUpdate
        }
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private TimeMode m_TimeMode = new TimeMode();
        [SerializeField] private RunMode m_RunIn = RunMode.MainUpdate;

        [SerializeField] private CameraTransition m_Transition = new CameraTransition();
        [SerializeField] private CameraAvoidClip m_AvoidClip = new CameraAvoidClip();

        // MEMBERS: -------------------------------------------------------------------------------
        
        private readonly CameraShakeSustain m_ShakeSustain = new CameraShakeSustain();
        private readonly CameraShakeBurst m_ShakeBurst = new CameraShakeBurst();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public CameraTransition Transition => this.m_Transition;
        public CameraAvoidClip AvoidClip => this.m_AvoidClip;
        
        public TimeMode Time => this.m_TimeMode;
        
        // EVENTS: --------------------------------------------------------------------------------
        
        public event Action<ShotCamera> EventCut;
        public event Action<ShotCamera> EventTransition;
        
        public event Action EventBeforeUpdate;
        public event Action EventAfterUpdate;
        
        // INITIALIZERS: --------------------------------------------------------------------------

        protected virtual void Awake()
        {
            this.m_Transition.EventCut += this.EventCut;
            this.m_Transition.EventTransition += this.EventTransition;
        }

        private void Start()
        {
            this.Transition.Initialize(this);
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        private void LateUpdate()
        {
            this.EventBeforeUpdate?.Invoke();

            if (this.m_RunIn == RunMode.MainUpdate)
            {
                this.Transition.NormalUpdate();
                Transform cameraTransform = this.transform;
            
                cameraTransform.position = this.Transition.Position;
                cameraTransform.rotation = this.Transition.Rotation;
            }
            
            this.UpdateShakeEffect();
            this.UpdateAvoidClipping();

            this.EventAfterUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            if (this.m_RunIn == RunMode.FixedUpdate)
            {
                this.Transition.FixedUpdate();
                Transform cameraTransform = this.transform;
            
                cameraTransform.position = this.Transition.Position;
                cameraTransform.rotation = this.Transition.Rotation;
            }
        }

        private void UpdateShakeEffect()
        {
            this.m_ShakeSustain.Update(this);
            this.m_ShakeBurst.Update(this);
            
            Vector3 shakeDeltaPosition = 
                this.m_ShakeSustain.AdditivePosition * ShakeEffect.COEF_SHAKE_POSITION +
                this.m_ShakeBurst.AdditivePosition * ShakeEffect.COEF_SHAKE_POSITION;
            
            Vector3 shakeDeltaRotation =
                this.m_ShakeSustain.AdditiveRotation * ShakeEffect.COEF_SHAKE_ROTATION +
                this.m_ShakeBurst.AdditiveRotation * ShakeEffect.COEF_SHAKE_ROTATION;

            Transform cameraTransform = this.transform;
            
            cameraTransform.localPosition += shakeDeltaPosition;
            cameraTransform.localEulerAngles += shakeDeltaRotation;
        }
        
        private void UpdateAvoidClipping()
        {
            if (this.Transition.CurrentShotCamera == null) return;
            
            Transform target = this.Transition.CurrentShotCamera.Target;
            transform.position = this.AvoidClip.Update(this, target);
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void AddSustainShake(int layer, float delay, float transition, ShakeEffect shakeEffect)
        {
            this.m_ShakeSustain.AddSustain(layer, delay, transition, shakeEffect);
        }
        
        public void RemoveSustainShake(int layer, float delay, float transition)
        {
            this.m_ShakeSustain.RemoveSustain(layer, delay, transition);            
        }

        public void AddBurstShake(float delay, float duration, ShakeEffect shakeEffect)
        {
            this.m_ShakeBurst.AddBurst(delay, duration, shakeEffect);
        }

        public void StopBurstShakes(float delay, float transition)
        {
            this.m_ShakeBurst.RemoveBursts(delay, transition);
        }

        // GIZMOS: --------------------------------------------------------------------------------

        private void OnDrawGizmos()
        {
            this.m_AvoidClip?.OnDrawGizmos(this);
        }

        private void OnDrawGizmosSelected()
        {
            this.m_AvoidClip?.OnDrawGizmosSelected(this);
        }
    }   
}
