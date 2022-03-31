using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class CameraTransition
    {
        private const float DEFAULT_SMOOTH_TIME = 0.1f;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private ShotCamera m_CurrentShotCamera;
        [SerializeField] private float m_SmoothTimePosition;
        [SerializeField] private float m_SmoothTimeRotation;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private TCamera m_Camera;
        private ShotCamera m_PreviousShotCamera;

        private float m_ChangeDuration;
        private float m_ChangeTime;

        private Vector3 m_PositionVelocity;
        private Quaternion m_RotationVelocity;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        
        public ShotCamera CurrentShotCamera
        {
            get => this.m_CurrentShotCamera;
            set => this.m_CurrentShotCamera = value;
        }

        public ShotCamera PreviousShotCamera => m_PreviousShotCamera;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<ShotCamera> EventCut;
        public event Action<ShotCamera> EventTransition;

        // INITIALIZERS: --------------------------------------------------------------------------

        public CameraTransition()
        {
            this.m_SmoothTimePosition = DEFAULT_SMOOTH_TIME;
            this.m_SmoothTimeRotation = DEFAULT_SMOOTH_TIME;
        }

        public void Initialize(TCamera camera)
        {
            this.m_Camera = camera;
            Transform cameraTransform = this.m_Camera.transform;
            
            this.Position = cameraTransform.position;
            this.Rotation = cameraTransform.rotation;

            if (this.m_CurrentShotCamera) this.m_CurrentShotCamera.OnEnableShot(camera);
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        public void NormalUpdate()
        {
            if (!this.m_CurrentShotCamera) return;

            float elapsedTime = this.m_Camera.Time.Time - this.m_ChangeTime;
            float t = this.m_ChangeDuration > float.Epsilon ? elapsedTime / this.m_ChangeDuration : 1f;

            this.Update(t, this.m_Camera.Time.DeltaTime);
        }

        public void FixedUpdate()
        {
            if (!this.m_CurrentShotCamera) return;
            
            float elapsedTime = this.m_Camera.Time.FixedTime - this.m_ChangeTime;
            float t = this.m_ChangeDuration > float.Epsilon ? elapsedTime / this.m_ChangeDuration : 1f;

            this.Update(t, this.m_Camera.Time.FixedDeltaTime);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Update(float t, float deltaTime)
        {
            Vector3 position = Vector3.Lerp(this.Position, this.m_CurrentShotCamera.Position, t);
            Quaternion rotation = Quaternion.Lerp(this.Rotation, this.m_CurrentShotCamera.Rotation, t);

            this.UpdatePosition(position, deltaTime);
            this.UpdateRotation(rotation, deltaTime);
        }
        
        private void UpdatePosition(Vector3 position, float deltaTime)
        {
            if (this.m_CurrentShotCamera.UseSmoothPosition && this.Position != position)
            {
                this.Position = Vector3.SmoothDamp(
                    this.Position, position,
                    ref this.m_PositionVelocity,
                    this.m_SmoothTimePosition,
                    Mathf.Infinity,
                    deltaTime
                );
            }
            else
            {
                this.Position = position;
            }
        }

        private void UpdateRotation(Quaternion rotation, float deltaTime)
        {
            if (this.m_CurrentShotCamera.UseSmoothRotation && this.Rotation != rotation)
            {
                this.Rotation = QuaternionUtils.SmoothDamp(
                    this.Rotation, rotation,
                    ref this.m_RotationVelocity,
                    this.m_SmoothTimeRotation,
                    deltaTime
                );
            }
            else
            {
                this.Rotation = rotation;
            }
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void ChangeToShot(ShotCamera shotCamera, float duration = 0f)
        {
            if (this.m_CurrentShotCamera)
            {
                this.m_CurrentShotCamera.OnDisableShot(this.m_Camera);
                this.m_PreviousShotCamera = this.m_CurrentShotCamera;
            }

            this.m_CurrentShotCamera = shotCamera;
            this.m_CurrentShotCamera.OnEnableShot(this.m_Camera);

            if (duration <= float.Epsilon)
            {
                this.Position = this.m_CurrentShotCamera.Position;
                this.Rotation = this.m_CurrentShotCamera.Rotation;
            }

            this.m_ChangeDuration = duration;
            this.m_ChangeTime = this.m_Camera.Time.Time;

            if (duration <= float.Epsilon) this.EventCut?.Invoke(this.m_CurrentShotCamera);
            else this.EventTransition?.Invoke(this.m_CurrentShotCamera);
        }

        public void ChangeToPreviousShot(float duration = 0f)
        {
            this.ChangeToShot(this.m_PreviousShotCamera, duration);
        }
    }
}
