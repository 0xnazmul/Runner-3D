using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Pivot")]
    [Image(typeof(IconRotationYaw), ColorTheme.Type.Green)]
    
    [Category("Pivot")]
    [Description("Rotates the Character towards the direction it moves")]

    [Serializable]
    public class UnitFacingPivot : TUnitFacing
    {
        protected override Vector3 GetDefaultDirection()
        {
            Vector3 driverDirection = Vector3.Scale(
                this.Character.Driver.WorldMoveDirection,
                Vector3Plane.NormalUp
            );

            return this.DecideDirection(driverDirection);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Pivot";
    }
}