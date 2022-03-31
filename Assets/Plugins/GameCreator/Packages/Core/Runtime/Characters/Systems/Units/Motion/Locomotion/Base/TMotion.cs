using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    public abstract class TMotion
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        private Vector3 m_Velocity;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        protected TUnitMotion Motion { get; private set; }

        protected Character Character { get; private set; }
        protected Transform Transform { get; private set; }

        protected Vector3 Velocity
        {
            get => m_Velocity;
            set => m_Velocity = value;
        }

        public int Priority { get; internal set; }

        // INITIALIZERS: --------------------------------------------------------------------------

        public void Initialize(TUnitMotion motion, int priority)
        {
            this.Priority = priority;
            this.Motion = motion;

            this.Character = motion.Character;
            this.Transform = motion.Transform;
        }

        // ABSTRACT METHODS: ----------------------------------------------------------------------
        
        public abstract Character.MovementType Update();

        // VIRTUAL METHODS: -----------------------------------------------------------------------

        public virtual Character.MovementType Stop()
        {
            this.Priority = -1;
            
            this.Motion.MoveDirection = Vector3.zero;
            this.Motion.MovePosition = this.Transform.position;

            return Character.MovementType.None;
        }

        public virtual void OnDrawGizmos()
        { }

        protected virtual void Setup()
        { }

        // PROTECTED METHOD: ----------------------------------------------------------------------

        protected Vector3 CalculateSpeed(Vector3 direction)
        {
            direction = direction.normalized * this.Motion.LinearSpeed;
            return direction;
        }

        protected Vector3 CalculateAcceleration(Vector3 tarDirection)
        {
            if (!this.Motion.UseAcceleration) return tarDirection;
        
            Vector3 curDirection = Vector3.Scale(
                Vector3Plane.NormalUp,
                this.Character.Driver.WorldMoveDirection
            );
        
            float smoothTime = tarDirection.sqrMagnitude < curDirection.sqrMagnitude
                ? 1f / this.Motion.Deceleration
                : 1f / this.Motion.Acceleration;
            
            curDirection = Vector3.SmoothDamp(
                curDirection,
                tarDirection,
                ref this.m_Velocity,
                smoothTime,
                Mathf.Infinity,
                this.Character.Time.DeltaTime
            );

            if (Mathf.Abs(curDirection.sqrMagnitude) < 0.01f &&
                Mathf.Abs(tarDirection.sqrMagnitude) < 0.01f)
            {
                curDirection = Vector3.zero;
            }

            return curDirection;
        }
    }
}