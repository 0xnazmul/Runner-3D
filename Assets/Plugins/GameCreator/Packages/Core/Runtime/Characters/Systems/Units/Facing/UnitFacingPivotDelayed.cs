using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Pivot Delayed")]
    [Image(typeof(IconRotationYaw), ColorTheme.Type.Green, typeof(OverlayDot))]
    
    [Category("Pivot Delayed")]
    [Description("Rotates the Character towards the direction it's moving after a delay")]
    
    [Serializable]
    public class UnitFacingPivotDelayed : TUnitFacing
    {
        [SerializeField, Min(0f)] private float m_Delay = 1.75f;
        [SerializeField, Range(0f, 180f)] private float m_DelayAngle = 30f;

        private float m_DirectionChangeTime;
        private bool m_WasDirectionChanged;
        
        protected override Vector3 GetDefaultDirection()
        {
            Vector3 currentDirection = this.Transform.TransformDirection(Vector3.forward);
            Vector3 driverDirection = Vector3.Scale(
                this.Character.Driver.WorldMoveDirection,
                Vector3Plane.NormalUp
            );

            if (driverDirection.magnitude > this.Character.Motion.LinearSpeed * 0.25f)
            {
                if (!this.m_WasDirectionChanged)
                {
                    m_DirectionChangeTime = this.Character.Time.Time;
                }
                
                this.m_WasDirectionChanged = true;
            }
            else
            {
                this.m_WasDirectionChanged = false;
                return currentDirection;
            }
            
            if (Vector3.Angle(currentDirection, driverDirection) > this.m_DelayAngle)
            {
                return this.DecideDirection(
                    this.Character.Time.Time - this.m_DirectionChangeTime < this.m_Delay
                        ? currentDirection
                        : driverDirection
                );
            }

            return driverDirection;
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Pivot Delayed";
    }
}