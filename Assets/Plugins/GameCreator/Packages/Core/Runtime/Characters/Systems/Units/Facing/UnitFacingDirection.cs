using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Towards Direction")]
    [Image(typeof(IconArrowCircleRight), ColorTheme.Type.Yellow)]
    
    [Category("Towards Direction")]
    [Description("Rotates the Character towards a specific world-space direction")]
    
    [Serializable]
    public class UnitFacingDirection : TUnitFacing
    {
        private Args args;

        public PropertyGetRotation direction = GetRotationDirection.CreateForward;

        protected override Vector3 GetDefaultDirection()
        {
            if (this.args == null) this.args = new Args(this.Character);
            
            Vector3 driverDirection = Vector3.Scale(
                this.direction.Get(this.args) * Vector3.forward,
                Vector3Plane.NormalUp
            );

            return this.DecideDirection(driverDirection);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Towards Direction";
    }
}