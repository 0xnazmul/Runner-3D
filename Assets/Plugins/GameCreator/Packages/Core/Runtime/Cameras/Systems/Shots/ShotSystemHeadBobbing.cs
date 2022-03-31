using System;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class ShotSystemHeadBobbing : TShotSystem
    {
        private const float BOB_SMOOTH_TIME = 0.5f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDecimal m_StepLength = GetDecimalDecimal.Create(0.75f);
        [SerializeField] private PropertyGetDecimal m_StepHeight = GetDecimalDecimal.Create(0.05f);
        [SerializeField] private PropertyGetDecimal m_StepWidth = GetDecimalDecimal.Create(0.1f);
        
        // MEMBERS: -------------------------------------------------------------------------------

        private float m_BobTarget = 0f;
        private float m_BobCurrent = 0f;
        
        private float m_BobVelocity = 0f;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ShotSystemHeadBobbing() : base()
        { }

        // IMPLEMENTS: ----------------------------------------------------------------------------
        
        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);
            
            ShotTypeFirstPerson shotTypeFirstPerson = shotType as ShotTypeFirstPerson;
            if (shotTypeFirstPerson == null) return;
            
            if (shotType.IsActive && this.Active)
            {
                Character character = shotTypeFirstPerson.Character;
                if (character == null || !character.Driver.IsGrounded) this.m_BobTarget = 0f;
                else this.m_BobTarget = this.GetStepSpeedCoefficient(shotTypeFirstPerson);
            }
            else
            {
                this.m_BobTarget = 0f;
            }
            
            this.m_BobCurrent = Mathf.SmoothDamp(
                this.m_BobCurrent, 
                this.m_BobTarget,
                ref this.m_BobVelocity,
                BOB_SMOOTH_TIME
            );
            
            Vector3 movement = new Vector3(
                this.BobStepBalance(shotTypeFirstPerson),
                this.BobStepHeight(shotTypeFirstPerson),
                0f
            );
            
            shotType.Position += movement;
        }
        
        // MAGIC NUMBERS: -------------------------------------------------------------------------

        private float GetStepFrequency(ShotTypeFirstPerson shotType)
        {
            Character character = shotType.Character;
            float stepLength = (float) this.m_StepLength.Get(shotType.Args);
            
            return character != null 
                ? Mathf.Clamp01(stepLength / character.Motion.LinearSpeed) 
                : 0f;
        }
        
        private float GetStepSpeedCoefficient(ShotTypeFirstPerson shotType)
        {
            Character character = shotType.Character;
            Vector3 velocity = Vector3.Scale(Vector3Plane.NormalUp, character.Driver.WorldMoveDirection);
            
            return character != null 
                ? Mathf.Clamp01(velocity.magnitude / character.Motion.LinearSpeed) 
                : 0f;
        }
        
        private float GetStepPeriod(ShotTypeFirstPerson shotType)
        {
            float speed = this.GetStepFrequency(shotType); 
            return shotType.ShotCamera.TimeMode.Time / speed;
        }
        
        // +--------------------------------------------------------------------------------------+
        // | y = cos(x * 2) - 1                                                                   |
        // +--------------------------------------------------------------------------------------+
        private float BobStepHeight(ShotTypeFirstPerson shotType)
        {
            float stepHeight = (float) this.m_StepHeight.Get(shotType.Args);
            float speed = this.GetStepSpeedCoefficient(shotType);
            float t = this.GetStepPeriod(shotType);
            
            return Mathf.Lerp(
                0f, 
                (Mathf.Cos(t * 2f) - 1f) * stepHeight * speed,
                t * this.m_BobCurrent
            );
        }

        // +--------------------------------------------------------------------------------------+
        // | y = sin(x)                                                                           |
        // +--------------------------------------------------------------------------------------+
        private float BobStepBalance(ShotTypeFirstPerson shotType)
        {
            float stepWidth = (float) this.m_StepWidth.Get(shotType.Args);
            float speed = this.GetStepSpeedCoefficient(shotType);
            float t = this.GetStepPeriod(shotType);

            return Mathf.Lerp(
                0f, 
                Mathf.Sin(t) * stepWidth * speed, 
                t * this.m_BobCurrent
            );
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
        
        private void DoDrawGizmos(TShotType shotType, Color color)
        {
            Gizmos.color = color;
        }
    }
}