using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Look at Target")]
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    
    [Category("Look at Target")]
    [Description("Rotates the Character towards a specific game object target")]
    
    [Serializable]
    public class UnitFacingTarget : TUnitFacing
    {
        private Args args;

        public PropertyGetGameObject target = GetGameObjectPlayer.Create();

        protected override Vector3 GetDefaultDirection()
        {
            if (this.args == null) this.args = new Args(this.Character);

            GameObject gameObject = this.target.Get(this.args);
            if (gameObject == null) return Vector3.zero;
            
            Vector3 driverDirection = Vector3.Scale(
                gameObject.transform.position - this.Transform.position,
                Vector3Plane.NormalUp
            );

            return this.DecideDirection(driverDirection);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Look at Target";
    }
}