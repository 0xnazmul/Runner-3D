using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Title("Lock On")]
    [Category("Lock On")]
    [Image(typeof(IconShotLockOn), ColorTheme.Type.Blue)]
    
    [Description("Follows an object from a distance and tracks another one, so both are framed")]
    
    [Serializable]
    public class ShotTypeLockOn : TShotTypeLook
    {
        [SerializeField] private ShotSystemZoom m_ShotSystemZoom;
        [SerializeField] private ShotSystemLockOn m_ShotSystemLockOn;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public ShotSystemZoom Zoom => m_ShotSystemZoom;
        public ShotSystemLockOn LockOn => m_ShotSystemLockOn;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotTypeLockOn()
        {
            this.Look = new ShotSystemLook(
                GetGameObjectInstance.Create(),
                GetOffsetNone.Create
            );
            
            this.m_ShotSystemZoom = new ShotSystemZoom();
            this.m_ShotSystemLockOn = new ShotSystemLockOn();
        }

        // OVERRIDERS: ----------------------------------------------------------------------------

        public override void OnAwake(ShotCamera shotCamera)
        {
            base.OnAwake(shotCamera);
            
            this.m_ShotSystemZoom?.OnAwake(this);
            this.m_ShotSystemLockOn?.OnAwake(this);
        }

        public override void OnStart(ShotCamera shotCamera)
        {
            base.OnStart(shotCamera);
            
            this.m_ShotSystemZoom?.OnStart(this);
            this.m_ShotSystemLockOn?.OnStart(this);
        }

        public override void OnDestroy(ShotCamera shotCamera)
        {
            base.OnDestroy(shotCamera);
            
            this.m_ShotSystemZoom?.OnDestroy(this);
            this.m_ShotSystemLockOn?.OnDestroy(this);
        }

        public override void OnEnable(TCamera camera)
        {
            base.OnEnable(camera);
            
            this.m_ShotSystemZoom?.OnEnable(this, camera);
            this.m_ShotSystemLockOn?.OnEnable(this, camera);
        }

        public override void OnDisable(TCamera camera)
        {
            base.OnDisable(camera);
            
            this.m_ShotSystemZoom?.OnDisable(this, camera);
            this.m_ShotSystemLockOn?.OnDisable(this, camera);
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            this.m_ShotSystemZoom?.OnUpdate(this);
            this.m_ShotSystemLockOn.SyncWithZoom(this);
            
            this.m_ShotSystemLockOn?.OnUpdate(this);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------
        
        public override void DrawGizmos()
        {
            base.DrawGizmos();
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemZoom.OnDrawGizmos(this);
            this.m_ShotSystemLockOn.OnDrawGizmos(this);
        }

        public override void DrawGizmosSelected()
        {
            base.DrawGizmosSelected();
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemZoom.OnDrawGizmosSelected(this);
            this.m_ShotSystemLockOn.OnDrawGizmosSelected(this);
        }
    }
}