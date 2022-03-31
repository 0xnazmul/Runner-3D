using System;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Kinematic")]
    [Image(typeof(IconCharacterRun), ColorTheme.Type.Green)]
    
    [Category("Kinematic")]
    [Description("Default animation system for characters")]

    [Serializable]
    public class UnitAnimimKinematic : TUnitAnimim
    {
        // STATIC PROPERTIES: ---------------------------------------------------------------------

        private static readonly int K_SPEED_Z = Animator.StringToHash("Speed-Z");
        private static readonly int K_SPEED_X = Animator.StringToHash("Speed-X");
        private static readonly int K_SPEED_Y = Animator.StringToHash("Speed-Y");
        
        private static readonly int K_SURFACE_SPEED = Animator.StringToHash("Movement");
        private static readonly int K_PIVOT_SPEED = Animator.StringToHash("Pivot");

        private static readonly int K_GROUNDED = Animator.StringToHash("Grounded");
        private static readonly int K_STAND = Animator.StringToHash("Stand");
        private static readonly int K_TIME_SCALE = Animator.StringToHash("Time-Scale");

        // MEMBERS: -------------------------------------------------------------------------------
        
        protected Dictionary<int, AnimFloat> m_SmoothParameters;
        protected Dictionary<int, AnimFloat> m_IndependentParameters;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        public override void OnStartup(Character character)
        {
            base.OnStartup(character);

            this.m_SmoothParameters = new Dictionary<int, AnimFloat>
            {
                { K_SPEED_Z, new AnimFloat(0f, this.m_SmoothTime) },
                { K_SPEED_X, new AnimFloat(0f, this.m_SmoothTime) },
                { K_SPEED_Y, new AnimFloat(0f, this.m_SmoothTime) },

                { K_SURFACE_SPEED, new AnimFloat(0f, this.m_SmoothTime) },
            };
            
            this.m_IndependentParameters = new Dictionary<int, AnimFloat>
            {
                { K_PIVOT_SPEED, new AnimFloat(0f, 0.01f) },
                { K_GROUNDED, new AnimFloat(1f, 0.2f) },
                { K_STAND, new AnimFloat(1f, 0.1f) },
            };
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (this.m_Animator == null) return;
            if (!this.m_Animator.gameObject.activeInHierarchy) return;

            this.m_Animator.updateMode = this.Character.Time.UpdateTime == TimeMode.UpdateMode.GameTime
                ? AnimatorUpdateMode.Normal
                : AnimatorUpdateMode.UnscaledTime;

            IUnitMotion motion = this.Character.Motion;
            IUnitDriver driver = this.Character.Driver;
            IUnitFacing facing = this.Character.Facing;

            Vector3 movementDirection = driver.LocalMoveDirection / motion.LinearSpeed;
            float movementMagnitude = Vector3.Scale(Vector3Plane.NormalUp, movementDirection).magnitude;

            float pivot = facing.PivotSpeed;

            foreach (KeyValuePair<int, AnimFloat> parameter in this.m_SmoothParameters)
            {
                parameter.Value.Smooth = this.m_SmoothTime;
            }

            // Update anim parameters:
            this.m_SmoothParameters[K_SPEED_Z].Update(movementDirection.z);
            this.m_SmoothParameters[K_SPEED_X].Update(movementDirection.x);
            this.m_SmoothParameters[K_SPEED_Y].Update(movementDirection.y);
            this.m_SmoothParameters[K_SURFACE_SPEED].Update(movementMagnitude);

            this.m_IndependentParameters[K_PIVOT_SPEED].Update(pivot);
            this.m_IndependentParameters[K_GROUNDED].Update(driver.IsGrounded);
            this.m_IndependentParameters[K_STAND].Update(motion.StandLevel.Current);

            // Update animator parameters:
            this.m_Animator.SetFloat(K_SPEED_Z, this.m_SmoothParameters[K_SPEED_Z].Current);
            this.m_Animator.SetFloat(K_SPEED_X, this.m_SmoothParameters[K_SPEED_X].Current);
            this.m_Animator.SetFloat(K_SPEED_Y, this.m_SmoothParameters[K_SPEED_Y].Current);
            this.m_Animator.SetFloat(K_SURFACE_SPEED, this.m_SmoothParameters[K_SURFACE_SPEED].Current);

            this.m_Animator.SetFloat(K_PIVOT_SPEED, this.m_IndependentParameters[K_PIVOT_SPEED].Current);
            this.m_Animator.SetFloat(K_GROUNDED, this.m_IndependentParameters[K_GROUNDED].Current);
            this.m_Animator.SetFloat(K_STAND, this.m_IndependentParameters[K_STAND].Current);
            this.m_Animator.SetFloat(K_TIME_SCALE, this.Character.Time.TimeScale);
        }
    }
}