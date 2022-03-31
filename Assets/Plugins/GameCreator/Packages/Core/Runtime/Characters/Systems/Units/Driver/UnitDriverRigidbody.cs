using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Rigidbody")]
    [Image(typeof(IconPhysics), ColorTheme.Type.Yellow)]
    
    [Category("Rigidbody")]
    [Description("Moves the Character using a physics based Rigidbody component")]
    
    [Serializable]
    public class UnitDriverRigidbody : TUnitDriver
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] protected float m_Drag = 0f;
        [SerializeField] protected PhysicMaterial m_Material;

        [SerializeField]
        private RigidbodyInterpolation m_Interpolation = RigidbodyInterpolation.Interpolate;
        
        [SerializeField] protected float m_GroundDistance = 0.1f;
        [SerializeField] protected LayerMask m_GroundMask = -1;

        // MEMBERS: -------------------------------------------------------------------------------

        protected CapsuleCollider m_Capsule;
        protected Rigidbody m_Rigidbody;

        protected float m_LastVerticalSpeed;

        protected bool m_IsGrounded;
        protected AnimFloat m_IsGroundedSmooth;
        protected AnimVector3 m_FloorNormal;
        
        protected float m_GroundTime = -100f;
        protected float m_JumpTime = -100f;

        // INTERFACE PROPERTIES: ------------------------------------------------------------------

        public override Vector3 WorldMoveDirection => this.m_Rigidbody.velocity;
        public override Vector3 LocalMoveDirection => this.Transform.InverseTransformDirection(
            this.WorldMoveDirection
        );

        public override float SkinWidth => 0f;
        public override bool IsGrounded => this.m_IsGrounded;
        public override Vector3 FloorNormal => this.m_FloorNormal.Current;

        // INITIALIZERS: --------------------------------------------------------------------------

        public UnitDriverRigidbody()
        {
            this.m_LastVerticalSpeed = 0f;
        }

        public override void OnStartup(Character character)
        {
            base.OnStartup(character);

            this.m_IsGroundedSmooth = new AnimFloat(1f, 0.01f);
            this.m_FloorNormal = new AnimVector3(Vector3.up, 0.05f);

            this.m_Capsule = this.Character.GetComponent<CapsuleCollider>();
            if (!this.m_Capsule)
            {
                GameObject instance = this.Character.gameObject;
                this.m_Capsule = instance.AddComponent<CapsuleCollider>();
                this.m_Capsule.hideFlags = HideFlags.HideInInspector;
            }
            
            this.m_Rigidbody = this.Character.GetComponent<Rigidbody>();
            if (!this.m_Rigidbody)
            {
                GameObject instance = this.Character.gameObject;
                this.m_Rigidbody = instance.AddComponent<Rigidbody>();
                this.m_Rigidbody.hideFlags = HideFlags.HideInInspector;
            }

            character.Ragdoll.EventBeforeStartRagdoll += this.OnStartRagdoll;
            character.Ragdoll.EventAfterStartRecover += this.OnEndRagdoll;

            this.m_Rigidbody.useGravity = false;
            this.m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            this.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public override void OnDispose(Character character)
        {
            base.OnDispose(character);

            UnityEngine.Object.Destroy(this.m_Capsule);
            UnityEngine.Object.Destroy(this.m_Rigidbody);
            
            character.Ragdoll.EventBeforeStartRagdoll -= this.OnStartRagdoll;
            character.Ragdoll.EventAfterStartRecover -= this.OnEndRagdoll;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        public override void OnUpdate()
        {
            this.UpdateProperties();
            this.UpdateJump(this.Character.Motion);
        }

        public override void OnFixedUpdate()
        {
            this.CheckGround(this.Character.Motion);
            this.UpdateGravity(this.Character.Motion);

            this.UpdateTranslation(this.Character.Motion);
        }

        protected virtual void UpdateProperties()
        {
            this.m_FloorNormal.UpdateWithDelta(this.Character.Time.DeltaTime);
            
            float height = this.Character.Motion.Height;
            float radius = this.Character.Motion.Radius;

            if (Math.Abs(this.m_Capsule.height - height) > float.Epsilon)
            {
                this.m_Capsule.height = height;   
            }

            if (Math.Abs(this.m_Capsule.radius - radius) < float.Epsilon)
            {
                this.m_Capsule.radius = radius;
            }
            
            if (this.m_Capsule.center != Vector3.zero)
            {
                this.m_Capsule.center = Vector3.zero;
            }

            if (this.m_Capsule.material != this.m_Material)
            {
                this.m_Capsule.material = this.m_Material;
            }

            if (this.m_Rigidbody.interpolation != this.m_Interpolation)
            {
                this.m_Rigidbody.interpolation = this.m_Interpolation;
            }
            
            if (Math.Abs(this.m_Rigidbody.mass - this.Character.Motion.Mass) > float.Epsilon)
            {
                this.m_Rigidbody.mass = this.Character.Motion.Mass;
            }
            
            if (Math.Abs(this.m_Rigidbody.drag - this.m_Drag) > float.Epsilon)
            {
                this.m_Rigidbody.drag = this.m_Drag;
            }
        }

        private RaycastHit[] m_HitsBuffer = new RaycastHit[1];

        protected virtual void CheckGround(IUnitMotion motion)
        {
            int hitCount = Physics.RaycastNonAlloc(
                this.Character.Feet + Vector3.up * this.m_GroundDistance, 
                Vector3.down,
                this.m_HitsBuffer,
                this.m_GroundDistance * 2f,
                this.m_GroundMask,
                QueryTriggerInteraction.Ignore
            );

            this.m_IsGrounded = hitCount > 0;
            this.m_FloorNormal.Target = this.m_IsGrounded
                ? this.m_HitsBuffer[0].normal
                : Vector3.up;

            float deltaTime = this.Character.Time.FixedDeltaTime;
            this.m_IsGroundedSmooth.UpdateWithDelta(
                this.m_IsGrounded ? 1f : 0f, 
                COYOTE_TIME, 
                deltaTime
            );
        }

        protected virtual void UpdateJump(IUnitMotion motion)
        {
            if (!motion.IsJumping) return;
            if (!motion.CanJump) return;
            
            bool jumpCooldown = this.m_JumpTime + motion.JumpCooldown < this.Character.Time.Time;
            if (!jumpCooldown) return;

            Vector3 velocity = this.m_Rigidbody.velocity;
            this.m_Rigidbody.velocity = new Vector3(velocity.x, 0f, velocity.z);
            
            this.m_Rigidbody.AddForce(
                Vector3.up * motion.IsJumpingForce, 
                ForceMode.VelocityChange
            );
            
            this.m_JumpTime = this.Character.Time.Time;
            this.Character.OnJump(motion.IsJumpingForce);
        }

        protected virtual void UpdateGravity(IUnitMotion motion)
        {
            Vector3 mass = Vector3.up * this.m_Rigidbody.mass;
            this.m_Rigidbody.AddForce(mass * this.Character.Motion.Gravity);

            if (this.m_IsGrounded)
            {
                if (this.Character.Time.Time - this.m_GroundTime > COYOTE_TIME)
                {
                    this.Character.OnLand(this.m_LastVerticalSpeed);
                }
                
                this.m_GroundTime = this.Character.Time.Time;
            }

            Vector3 velocity = this.m_Rigidbody.velocity;
            this.m_Rigidbody.velocity = new Vector3(
                velocity.x,
                Mathf.Max(velocity.y, motion.TerminalVelocity),
                velocity.z
            );

            this.m_LastVerticalSpeed = this.m_Rigidbody.velocity.y;
        }

        protected virtual void UpdateTranslation(IUnitMotion motion)
        {
            Vector3 kinetic = motion.MovementType switch
            {
                Character.MovementType.MoveToDirection => this.UpdateMoveToDirection(motion),
                Character.MovementType.MoveToPosition => this.UpdateMoveToPosition(motion),
                _ => Vector3.zero
            };

            Vector3 rootMotion = this.Character.Animim.RootMotionDeltaPosition;
            Vector3 movement = Vector3.Lerp(kinetic, rootMotion, this.Character.RootMotion);
            
            this.m_Rigidbody.velocity = new Vector3(
                movement.x,
                this.m_Rigidbody.velocity.y,
                movement.z
            );
        }

        // POSITION METHODS: ----------------------------------------------------------------------

        protected virtual Vector3 UpdateMoveToDirection(IUnitMotion motion)
        {
            return motion.MoveDirection;
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
            
            return motion.MoveDirection.normalized * velocity;
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
        
        private void OnStartRagdoll()
        {
            this.m_Rigidbody.isKinematic = true;
            this.m_Capsule.enabled = false;
        }
        
        private void OnEndRagdoll()
        {
            this.m_Capsule.enabled = true;
            this.m_Rigidbody.isKinematic = false;
            this.m_Rigidbody.velocity = Vector3.zero;
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