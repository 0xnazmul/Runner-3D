using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Third Person")]
    [Category("Third Person")]
    [Image(typeof(IconShotThirdPerson), ColorTheme.Type.Blue)]
    
    [Description("Follows the target from a certain distance while allowing to orbit around it")]
    
    [Serializable]
    public class ShotTypeThirdPerson : TShotTypeLook
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private ShotSystemZoom m_ShotSystemZoom;
        [SerializeField] private ShotSystemOrbit m_ShotSystemOrbit;

        // PROPERTIES: ----------------------------------------------------------------------------

        public ShotSystemZoom Zoom => m_ShotSystemZoom;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotTypeThirdPerson()
        {
            this.m_ShotSystemZoom = new ShotSystemZoom();
            this.m_ShotSystemOrbit = new ShotSystemOrbit();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------

        public void AddRotation(float pitch, float yaw)
        {
            this.m_ShotSystemOrbit.Pitch += pitch;
            this.m_ShotSystemOrbit.Yaw += yaw;
        }

        // OVERRIDERS: ----------------------------------------------------------------------------

        public override void OnAwake(ShotCamera shotCamera)
        {
            base.OnAwake(shotCamera);
            
            this.m_ShotSystemZoom?.OnAwake(this);
            this.m_ShotSystemOrbit?.OnAwake(this);
        }

        public override void OnStart(ShotCamera shotCamera)
        {
            base.OnStart(shotCamera);
            
            this.m_ShotSystemZoom?.OnStart(this);
            this.m_ShotSystemOrbit?.OnStart(this);
        }

        public override void OnDestroy(ShotCamera shotCamera)
        {
            base.OnDestroy(shotCamera);
            
            this.m_ShotSystemZoom?.OnDestroy(this);
            this.m_ShotSystemOrbit?.OnDestroy(this);
        }

        public override void OnEnable(TCamera camera)
        {
            base.OnEnable(camera);
            
            this.m_ShotSystemZoom?.OnEnable(this, camera);
            this.m_ShotSystemOrbit?.OnEnable(this, camera);
        }

        public override void OnDisable(TCamera camera)
        {
            base.OnDisable(camera);
            
            this.m_ShotSystemZoom?.OnDisable(this, camera);
            this.m_ShotSystemOrbit?.OnDisable(this, camera);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            this.m_ShotSystemZoom?.OnUpdate(this);
            this.m_ShotSystemOrbit.SyncWithZoom(this.m_ShotSystemZoom);
            
            this.m_ShotSystemOrbit?.OnUpdate(this);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public override void DrawGizmos()
        {
            base.DrawGizmos();
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemZoom.OnDrawGizmos(this);
            this.m_ShotSystemOrbit.OnDrawGizmos(this);
        }

        public override void DrawGizmosSelected()
        {
            base.DrawGizmosSelected();
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemZoom.OnDrawGizmosSelected(this);
            this.m_ShotSystemOrbit.OnDrawGizmosSelected(this);
        }
    }
}