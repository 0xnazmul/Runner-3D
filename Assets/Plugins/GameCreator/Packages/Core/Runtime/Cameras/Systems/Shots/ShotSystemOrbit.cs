using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class ShotSystemOrbit : TShotSystem
    {
        private const string INPUT_ROTATE = "Orbit";
        
        protected static readonly Vector3 GIZMO_SIZE_CUBE = Vector3.one * 0.1f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetGameObject m_OrbitTarget = 
            GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetOffset m_OrbitOffset = new PropertyGetOffset(
            new GetOffsetLocalTarget(new Vector3(0f, 0.5f, 0f))
        );

        [SerializeField] private InputPropertyValueVector2 m_InputOrbit;

        [SerializeField]
        private PropertyGetDecimal m_InputSensitivityX = GetDecimalDecimal.Create(180f);
        
        [SerializeField]
        private PropertyGetDecimal m_InputSensitivityY = GetDecimalDecimal.Create(180f);

        [SerializeField, Range(1f, 179f)] private float m_MaxPitch = 60f;
        [SerializeField] private float m_MaxRadius = 5f;
        [SerializeField] private float m_SmoothTime = 0.15f;

        [SerializeField] private bool m_AlignWithTarget = true;
        [SerializeField] private float m_AlignDelay = 3f;
        [SerializeField] private float m_AlignSmoothTime = 3f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Vector3 m_LastTargetPosition = Vector3.zero;
        
        private Vector2 m_OrbitAnglesCurrent = new Vector2(45f, 0f);
        private Vector2 m_OrbitAnglesTarget = new Vector2(45f, 0f);

        private float m_VelocityX;
        private float m_VelocityY;
        
        private float m_Radius;

        private float m_LastInputTime;
        private float m_AlignVelocity;

        // PROPERTIES: ----------------------------------------------------------------------------

        public float Pitch
        {
            get => this.m_OrbitAnglesTarget.x;
            set => this.m_OrbitAnglesTarget.x = value;
        }
        
        public float Yaw
        {
            get => this.m_OrbitAnglesTarget.y;
            set => this.m_OrbitAnglesTarget.y = value;
        }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotSystemOrbit()
        {
            this.m_InputOrbit = InputValueVector2MotionSecondary.Create();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void SyncWithZoom(ShotSystemZoom shotSystemZoom)
        {
            float maxRadius = Mathf.Max(0f, this.m_MaxRadius - shotSystemZoom.MinDistance);
            this.m_Radius = shotSystemZoom.MinDistance + maxRadius * shotSystemZoom.Level;
        }

        public void SetRotation(Quaternion rotation)
        {
            this.m_OrbitAnglesTarget = rotation.eulerAngles;
            this.m_OrbitAnglesCurrent = rotation.eulerAngles;
        }

        public void SetDirection(Vector3 direction)
        {
            this.SetRotation(Quaternion.LookRotation(direction, Vector3.up));
        }

        // IMPLEMENTS: ----------------------------------------------------------------------------
        
        public override void OnAwake(TShotType shotType)
        {
            base.OnAwake(shotType);
            this.m_InputOrbit.OnStartup();
        }

        public override void OnDestroy(TShotType shotType)
        {
            base.OnDestroy(shotType);
            this.m_InputOrbit.OnDispose();
        }

        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);
            this.m_InputOrbit.OnUpdate();
            
            if (shotType.IsActive && this.Active)
            {
                Vector2 deltaInput = this.m_InputOrbit.Read();

                float sensitivityX = (float) this.m_InputSensitivityX.Get(shotType.Args);
                float sensitivityY = (float) this.m_InputSensitivityY.Get(shotType.Args);

                if (deltaInput == Vector2.zero) this.ComputeAlign(shotType);  
                else this.ComputeInput(shotType, new Vector2(
                    deltaInput.x * shotType.ShotCamera.TimeMode.DeltaTime * sensitivityX, 
                    deltaInput.y * shotType.ShotCamera.TimeMode.DeltaTime * sensitivityY
                ));
            }

            this.ConstrainTargetAngles();
            
            this.m_OrbitAnglesCurrent = new Vector2(
                this.GetRotationDamp(
                    this.m_OrbitAnglesCurrent.x, 
                    this.m_OrbitAnglesTarget.x,
                    ref this.m_VelocityX,
                    this.m_SmoothTime,
                    shotType.ShotCamera.TimeMode.DeltaTime),
                this.GetRotationDamp(
                    this.m_OrbitAnglesCurrent.y, 
                    this.m_OrbitAnglesTarget.y, 
                    ref this.m_VelocityY,
                    this.m_SmoothTime,
                    shotType.ShotCamera.TimeMode.DeltaTime)
            );
            
            this.m_LastTargetPosition = this.GetTargetPosition(shotType);
            
            Quaternion lookRotation = Quaternion.Euler(this.m_OrbitAnglesCurrent);
            Vector3 lookDirection = lookRotation * Vector3.forward;

            Vector3 position = this.m_LastTargetPosition - lookDirection * this.m_Radius;
            shotType.Position = position;
        }

        public override void OnEnable(TShotType shotType, TCamera camera)
        {
            base.OnEnable(shotType, camera);
            this.m_InputOrbit?.Enable();

            this.SetRotation(camera.transform.rotation);
        }

        public override void OnDisable(TShotType shotType, TCamera camera)
        {
            base.OnDisable(shotType, camera);
            this.m_InputOrbit?.Disable();
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        public override void OnDrawGizmos(TShotType shotType)
        {
            base.OnDrawGizmos(shotType);
            this.DoDrawGizmos(shotType, GIZMOS_COLOR_GHOST);
        }

        public override void OnDrawGizmosSelected(TShotType shotType)
        {
            base.OnDrawGizmosSelected(shotType);
            this.DoDrawGizmos(shotType, GIZMOS_COLOR_ACTIVE);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private float GetRotationDamp(float current, float target, ref float velocity, 
            float smoothTime, float deltaTime)
        {
            return Mathf.SmoothDampAngle(
                current,
                target,
                ref velocity,
                smoothTime,
                Mathf.Infinity,
                deltaTime
            );
        }
        
        private void ConstrainTargetAngles()
        {
            float angle = this.m_MaxPitch / 2f;
            m_OrbitAnglesTarget.x = Mathf.Clamp(m_OrbitAnglesTarget.x, -angle, angle);

            if (m_OrbitAnglesTarget.y < 0f) m_OrbitAnglesTarget.y += 360f;
            if (m_OrbitAnglesTarget.y >= 360f) m_OrbitAnglesTarget.y -= 360f;
        }
        
        private Vector3 GetTargetPosition(TShotType shotType)
        {
            GameObject target = this.m_OrbitTarget.Get(shotType.Args);
            if (target == null) return this.m_LastTargetPosition;
            
            return target.transform.position + this.m_OrbitOffset.Get(shotType.Args);
        }
        
        private bool GetTargetRotation(TShotType shotType, out Quaternion targetRotation)
        {
            targetRotation = Quaternion.identity;
            GameObject target = this.m_OrbitTarget.Get(shotType.Args);
            if (target == null) return false;

            targetRotation = target.transform.rotation;
            return true;
        }

        private void ComputeAlign(TShotType shotType)
        {
            float time = shotType.ShotCamera.TimeMode.Time;
            
            if (this.m_AlignWithTarget && time - this.m_LastInputTime > this.m_AlignDelay)
            {
                if (this.GetTargetRotation(shotType, out Quaternion targetRotation))
                {
                    this.m_OrbitAnglesTarget.y = this.GetRotationDamp(
                        this.m_OrbitAnglesTarget.y,
                        targetRotation.eulerAngles.y,
                        ref this.m_AlignVelocity,
                        this.m_AlignSmoothTime,
                        shotType.ShotCamera.TimeMode.DeltaTime
                    );
                }
            }
        }
        
        private void ComputeInput(TShotType shotType, Vector2 deltaInput)
        {
            this.m_LastInputTime = shotType.ShotCamera.TimeMode.Time;
            this.m_AlignVelocity = 0f;
            this.m_OrbitAnglesTarget += new Vector2(
                deltaInput.y,
                deltaInput.x
            );
        }
        
        private void DoDrawGizmos(TShotType shotType, Color color)
        {
            Gizmos.color = color;
            if (!this.Active) return;
            
            Vector3 positionMin = this.GetTargetPosition(shotType);
            Quaternion lookRotation = Quaternion.Euler(this.m_OrbitAnglesCurrent);
            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 positionMax = positionMin - lookDirection * this.m_MaxRadius;
            
            Gizmos.DrawWireCube(positionMin, GIZMO_SIZE_CUBE);
            Gizmos.DrawWireCube(positionMax, GIZMO_SIZE_CUBE);
            Gizmos.DrawLine(positionMin, positionMax);
            
            GizmosExtension.Circle(positionMin, this.m_Radius);
            GizmosExtension.Circle(positionMin, this.m_MaxRadius);
        }
    }
}