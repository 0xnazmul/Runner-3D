using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Object Direction")]
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Category("Object Direction")]
    [Description("Looks at the same direction as another game object")]

    [Serializable]
    public class UnitFacingObjectDirection : TUnitFacing
    {
        [SerializeField]
        private PropertyGetGameObject m_DirectionOf = GetGameObjectMainCamera.Create();
        
        protected override Vector3 GetDefaultDirection()
        {
            GameObject gameObject = this.m_DirectionOf.Get(this.Character.gameObject);
            Vector3 driverDirection = gameObject != null
                ? Vector3.Scale(gameObject.transform.forward, Vector3Plane.NormalUp)
                : Vector3.zero;

            return this.DecideDirection(driverDirection);
        }
        
        // STRING: --------------------------------------------------------------------------------

        public override string ToString() => "Object Direction";
    }
}