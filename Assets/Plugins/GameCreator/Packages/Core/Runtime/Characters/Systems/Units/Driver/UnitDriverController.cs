using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Character Controller")]
    [Image(typeof(IconCapsuleSolid), ColorTheme.Type.Green)]
    
    [Category("Character Controller")]
    [Description("Moves the Character using Unity's default Character Controller")]
    
    [Serializable]
    public class UnitDriverController : TUnitDriver
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected float m_PushForce = 1.0f;
        [SerializeField] protected float m_MaxSlope = 45f;

        // MEMBERS: -------------------------------------------------------------------------------

        protected CharacterController m_Controller;

        protected Vector3 m_MoveDirection;
        protected float m_VerticalSpeed;

        protected AnimFloat m_IsGrounded;
        protected AnimVector3 m_FloorNormal;

        protected float m_GroundTime = -100f;
        protected float m_JumpTime = -100f;

        protected DriverControllerComponent m_Helper;

        // INTERFACE PROPERTIES: ------------------------------------------------------------------

        public override Vector3 WorldMoveDirection => this.m_Controller.velocity;
        public override Vector3 LocalMoveDirection => this.Transform.InverseTransformDirection(
            this.WorldMoveDirection
        );

        public override float SkinWidth => this.m_Controller.skinWidth;
        public override bool IsGrounded => this.m_Controller.isGrounded;
        public override Vector3 FloorNormal => this.m_FloorNormal.Current;

        // INITIALIZERS: --------------------------------------------------------------------------

        public UnitDriverController()
        {
            this.m_MoveDirection = Vector3.zero;
            this.m_VerticalSpeed = 0f;
        }

        public override void OnStartup(Character character)
        {
            base.OnStartup(character);

            this.m_IsGrounded = new AnimFloat(1f, 0.01f);
            this.m_FloorNormal = new AnimVector3(Vector3.up, 0.05f);

            this.m_Controller = this.Character.GetComponent<CharacterController>();
            if (!this.m_Controller)
            {
                GameObject instance = this.Character.gameObject;
                this.m_Controller = instance.AddComponent<CharacterController>();
                this.m_Controller.hideFlags = HideFlags.HideInInspector;
            }
            
            this.m_Helper = DriverControllerComponent.Register(
                this.Character,
                this.OnControllerColliderHit
            );

            character.Ragdoll.EventBeforeStartRagdoll += this.OnStartRagdoll;
            character.Ragdoll.EventAfterStartRecover += this.OnEndRagdoll;
        }

        public override void OnDispose(Character character)
        {
            base.OnDispose(character);

            UnityEngine.Object.Destroy(this.m_Helper);
            UnityEngine.Object.Destroy(this.m_Controller);
            
            character.Ragdoll.EventBeforeStartRagdoll -= this.OnStartRagdoll;
            character.Ragdoll.EventAfterStartRecover -= this.OnEndRagdoll;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        public override void OnUpdate()
        {
            this.UpdateProperties();

            this.UpdateGravity(this.Character.Motion);
            this.UpdateJump(this.Character.Motion);

            this.UpdateTranslation(this.Character.Motion);
        }

        protected virtual void UpdateProperties()
        {
            this.m_FloorNormal.UpdateWithDelta(this.Character.Time.DeltaTime);
            this.m_MoveDirection = Vector3.zero;
            this.m_IsGrounded.Update(this.m_Controller.isGrounded, COYOTE_TIME);

            if (Math.Abs(this.m_Controller.slopeLimit - this.m_MaxSlope) > float.Epsilon)
            {
                this.m_Controller.slopeLimit = this.m_MaxSlope;
            }
            
            float height = this.Character.Motion.Height;
            float radius = this.Character.Motion.Radius;

            if (Math.Abs(this.m_Controller.height - height) > float.Epsilon)
            {
                this.m_Controller.height = height;   
            }

            if (Math.Abs(this.m_Controller.radius - radius) > float.Epsilon)
            {
                this.m_Controller.radius = radius;
            }

            if (this.m_Controller.center != Vector3.zero)
            {
                this.m_Controller.center = Vector3.zero;   
            }
        }

        protected virtual void UpdateJump(IUnitMotion motion)
        {
            if (!motion.IsJumping) return;
            if (!motion.CanJump) return;
            
            bool jumpCooldown = this.m_JumpTime + motion.JumpCooldown < this.Character.Time.Time;
            if (!jumpCooldown) return;
            
            this.m_VerticalSpeed = motion.IsJumpingForce;
            this.m_JumpTime = this.Character.Time.Time;
            this.Character.OnJump(motion.IsJumpingForce);
        }

        protected virtual void UpdateGravity(IUnitMotion motion)
        {
            this.m_VerticalSpeed += motion.Gravity * this.Character.Time.DeltaTime;

            if (this.m_Controller.isGrounded)
            {
                if (this.Character.Time.Time - this.m_GroundTime > COYOTE_TIME)
                {
                    this.Character.OnLand(this.m_VerticalSpeed);
                }
                
                this.m_GroundTime = this.Character.Time.Time;

                this.m_VerticalSpeed = Mathf.Max(
                    this.m_VerticalSpeed, motion.Gravity
                );
            }

            this.m_VerticalSpeed = Mathf.Max(
                this.m_VerticalSpeed,
                motion.TerminalVelocity
            );
        }

        protected virtual void UpdateTranslation(IUnitMotion motion)
        {
            Vector3 movement = Vector3.up * (this.m_VerticalSpeed * this.Character.Time.DeltaTime);

            Vector3 kinetic = motion.MovementType switch
            {
                Character.MovementType.MoveToDirection => this.UpdateMoveToDirection(motion),
                Character.MovementType.MoveToPosition => this.UpdateMoveToPosition(motion),
                _ => Vector3.zero
            };

            Vector3 rootMotion = this.Character.Animim.RootMotionDeltaPosition;
            movement += Vector3.Lerp(kinetic, rootMotion, this.Character.RootMotion);
            
            if (this.m_Controller.enabled) this.m_Controller.Move(movement);
        }

        // POSITION METHODS: ----------------------------------------------------------------------

        protected virtual Vector3 UpdateMoveToDirection(IUnitMotion motion)
        {
            this.m_MoveDirection = motion.MoveDirection;
            return this.m_MoveDirection * this.Character.Time.DeltaTime;
        }

        protected virtual Vector3 UpdateMoveToPosition(IUnitMotion motion)
        {
            float distance = Vector3.Distance(this.Character.Feet, motion.MovePosition);
            float brakeRadiusHeuristic = Math.Max(motion.Height, motion.Radius * 2f);
            float velocity = motion.MoveDirection.magnitude;
            
            if (distance < brakeRadiusHeuristic)
            {
                velocity = Mathf.Lerp(
                    motion.LinearSpeed, motion.LinearSpeed * 0.25f,
                    1f - Mathf.Clamp01(distance / brakeRadiusHeuristic)
                );
            }
            
            this.m_MoveDirection = motion.MoveDirection;
            return this.m_MoveDirection.normalized * (velocity * this.Character.Time.DeltaTime);
        }

        // INTERFACE METHODS: ---------------------------------------------------------------------

        public override void SetPosition(Vector3 position)
        {
            position += Vector3.up * (this.Character.Motion.Height * 0.5f);
            this.Transform.position = position;
        }

        public override void SetRotation(Quaternion rotation)
        {
            this.Transform.rotation = rotation;
        }

        public override void AddPosition(Vector3 amount)
        {
            this.Transform.position += amount;
        }

        public override void AddRotation(Quaternion amount)
        {
            this.Transform.rotation *= amount;
        }

        // CALLBACK METHODS: ----------------------------------------------------------------------

        protected virtual void OnControllerColliderHit(ControllerColliderHit hit)
        {
            this.m_FloorNormal.Target = hit.normal;

            if (this.m_PushForce >= float.Epsilon)
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                if (angle > 90f) return;
                if (angle < 5f) return;

                Rigidbody hitRigidbody = hit.collider.attachedRigidbody;
                if (hitRigidbody && !hitRigidbody.isKinematic)
                {
                    Vector3 force = hit.controller.velocity * this.m_PushForce;
                    force /= this.Character.Time.FixedDeltaTime;

                    hitRigidbody.AddForceAtPosition(force, hit.point, ForceMode.Force);
                }
            }
        }
        
        private void OnStartRagdoll()
        {
            this.m_Controller.enabled = false;
        }
        
        private void OnEndRagdoll()
        {
            this.m_Controller.enabled = true;
            this.m_Controller.Move(Vector3.zero);
            this.m_MoveDirection = Vector3.zero;
        }

        // GIZMOS: --------------------------------------------------------------------------------

        public override void OnDrawGizmos(Character character)
        {
            if (!Application.isPlaying) return;

            IUnitMotion motion = character.Motion;
            if (motion == null) return;

            switch (motion.MovementType)
            {
                case Character.MovementType.MoveToPosition:
                    this.OnDrawGizmosToTarget(motion);
                    break;
            }
        }

        protected void OnDrawGizmosToTarget(IUnitMotion motion)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.Character.Feet, motion.MovePosition);
        }
    }
}