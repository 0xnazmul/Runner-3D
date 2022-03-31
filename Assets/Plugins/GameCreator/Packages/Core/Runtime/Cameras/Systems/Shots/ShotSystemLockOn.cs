using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class ShotSystemLockOn : TShotSystem
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_Anchor = GetGameObjectPlayer.Create();

        [SerializeField]
        private PropertyGetOffset m_AnchorOffset = new PropertyGetOffset(
            new GetOffsetLocalSelf(new Vector3(0f, 0.25f, -1f))
        );
        
        [SerializeField] private PropertyGetDecimal m_Distance = GetDecimalDecimal.Create(5f);

        // MEMBERS: -------------------------------------------------------------------------------

        private Vector3 m_LastPosition = Vector3.zero;

        private float m_Radius;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotSystemLockOn()
        { }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void SyncWithZoom(ShotTypeLockOn shotLockOn)
        {
            float distance = (float) this.m_Distance.Get(shotLockOn.Args);
            float maxRadius = Mathf.Max(0f, distance - shotLockOn.Zoom.MinDistance);
            
            this.m_Radius = shotLockOn.Zoom.MinDistance + maxRadius * shotLockOn.Zoom.Level;
        }

        // IMPLEMENTS: ----------------------------------------------------------------------------

        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);

            Vector3 positionTarget = this.GetTargetPosition(shotType as TShotTypeLook);
            Vector3 positionAnchor = this.GetAnchorPosition(shotType);

            Vector3 direction = positionTarget - positionAnchor;
            Vector3 position = positionAnchor - direction.normalized * this.m_Radius;
            
            this.m_LastPosition = positionAnchor;
            shotType.Position = position;
        }

        // GIZMOS: --------------------------------------------------------------------------------

        public override void OnDrawGizmos(TShotType shotType)
        {
            base.OnDrawGizmos(shotType);
            this.DoDrawGizmos(shotType as TShotTypeLook, GIZMOS_COLOR_GHOST);
        }

        public override void OnDrawGizmosSelected(TShotType shotType)
        {
            base.OnDrawGizmosSelected(shotType);
            this.DoDrawGizmos(shotType as TShotTypeLook, GIZMOS_COLOR_ACTIVE);
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private float GetRotationDamp(float current, float target, ref float velocity, 
            float smoothTime, float deltaTime)
        {
            return Mathf.SmoothDampAngle(
                current,
                target,
                ref velocity,
                smoothTime,
                Mathf.Infinity,
                deltaTime
            );
        }

        private Vector3 GetTargetPosition(TShotTypeLook shotType)
        {
            return shotType.Look.GetLookPosition(shotType);
        }
        
        private Vector3 GetAnchorPosition(TShotType shotType)
        {
            GameObject anchor = this.m_Anchor.Get(shotType.Args);
            if (anchor == null) return this.m_LastPosition;
            
            return anchor.transform.position + this.m_AnchorOffset.Get(shotType.Args);
        }

        private void DoDrawGizmos(TShotTypeLook shotType, Color color)
        {
            Gizmos.color = color;
            if (!this.Active) return;
            
            Vector3 positionTarget = this.GetTargetPosition(shotType);
            Vector3 positionAnchor = this.GetAnchorPosition(shotType);

            Gizmos.DrawSphere(positionTarget, 0.05f);
            Gizmos.DrawSphere(positionAnchor, 0.10f);
            
            Gizmos.DrawLine(positionTarget, positionAnchor);
            Gizmos.DrawLine(positionAnchor, shotType.Position);
        }
    }
}