using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    public class MotionToMarker : TMotionTarget<Marker>
    {
        private Vector3 m_LastKnownPosition;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected override bool HasTarget => this.m_Target != null;

        protected override Vector3 Position
        {
            get
            {
                if (this.m_Target != null) this.m_LastKnownPosition = this.m_Target.Position;
                return this.m_LastKnownPosition;
            }
        }

        protected override Vector3 Direction => this.m_Target != null 
            ? this.m_Target.Direction 
            : default;
    }
}