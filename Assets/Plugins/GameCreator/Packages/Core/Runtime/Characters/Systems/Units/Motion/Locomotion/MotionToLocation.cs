using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public class MotionToLocation : TMotionTarget<Location>
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        protected override bool HasTarget => this.m_Target.HasPosition;

        protected override Vector3 Position => this.m_Target.HasPosition 
            ? this.m_Target.Position
            : default;

        protected override Vector3 Direction => this.m_Target.HasRotation 
            ? this.m_Target.Rotation * Vector3.forward
            : default;
    }
}