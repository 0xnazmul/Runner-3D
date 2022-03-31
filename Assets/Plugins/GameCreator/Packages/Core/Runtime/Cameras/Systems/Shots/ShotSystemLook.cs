using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class ShotSystemLook : TShotSystem
    {
        protected static readonly Vector3 GIZMO_SIZE_CUBE = Vector3.one * 0.1f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField]
        private PropertyGetGameObject m_LookTarget = GetGameObjectPlayer.Create();

        [SerializeField] 
        private PropertyGetOffset m_LookOffset = GetOffsetNone.Create;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        public ShotSystemLook() : base()
        { }

        public ShotSystemLook(PropertyGetGameObject target, PropertyGetOffset offset) : this()
        {
            this.m_LookTarget = target;
            this.m_LookOffset = offset;
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public Transform GetLookTarget(IShotType shotType)
        {
            if (!this.Active) return null;

            GameObject target = this.m_LookTarget.Get(shotType.ShotCamera);
            return target != null ? target.transform : null;
        }

        public Vector3 GetLookOffset(IShotType shotType)
        {
            Transform target = this.GetLookTarget(shotType);
            return target != null 
                ? this.m_LookOffset.Get(target) 
                : default;
        }

        public Vector3 GetLookPosition(IShotType shotType)
        {
            Transform target = this.GetLookTarget(shotType);
            return target != null
                ? target.position + this.GetLookOffset(shotType)
                : default;
        }

        // IMPLEMENTS: ----------------------------------------------------------------------------
        
        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);
            
            if (shotType.IsActive && this.Active)
            {
                Transform value = this.GetLookTarget(shotType);
                if (value != null && value != shotType.ShotCamera.transform)
                {
                    Vector3 direction = this.GetLookPosition(shotType) - shotType.Position; 
                    shotType.Rotation = Quaternion.LookRotation(direction);
                }
            }
        }
        
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
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void DoDrawGizmos(TShotType shotType, Color color)
        {
            Gizmos.color = color;
            if (!this.Active) return;
            
            Transform target = this.GetLookTarget(shotType);
            if (target == null || target.position == shotType.Position) return;

            Vector3 look = this.GetLookPosition(shotType);
            
            Gizmos.DrawWireCube(look, GIZMO_SIZE_CUBE);
            Gizmos.DrawLine(shotType.Position, look);
        }
    }
}