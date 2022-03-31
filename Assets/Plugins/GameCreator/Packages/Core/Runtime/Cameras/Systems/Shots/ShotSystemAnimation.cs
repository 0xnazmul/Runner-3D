using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class ShotSystemAnimation : TShotSystem
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private float m_Duration = 3f;
        [SerializeField] private Easing.Type m_Easing = Easing.Type.QuadInOut;
        
        [SerializeField] private Bezier m_Path = new Bezier(
            new Vector3( 0f, 0f, -2f), // PointA 
            new Vector3( 0f, 0f,  2f), // PointB 
            new Vector3(-2f, 0f,  1f), // ControlA
            new Vector3(-2f, 0f, -1f)  // ControlB
        );
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        private float m_StartTime;

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public override void OnEnable(TShotType shotType, TCamera camera)
        {
            base.OnEnable(shotType, camera);
            this.m_StartTime = shotType.ShotCamera.TimeMode.Time;
        }

        public override void OnUpdate(TShotType shotType)
        {
            base.OnUpdate(shotType);

            if (shotType.IsActive && this.Active)
            {
                float elapsed = shotType.ShotCamera.TimeMode.Time - this.m_StartTime;
                float t = Easing.GetEase(this.m_Easing, 0f, 1f, elapsed / this.m_Duration);

                Vector3 position = this.m_Path.Get(t);
                shotType.Position = shotType.ShotCamera.transform.TransformPoint(position);
            }
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
            if (!this.Active) return;
            
            this.m_Path.DrawGizmos(shotType.ShotCamera.transform);
        }
    }
}