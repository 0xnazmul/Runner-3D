using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Title("Animation")]
    [Category("Animation")]
    [Image(typeof(IconShotAnimation), ColorTheme.Type.Blue)]
    
    [Description("Plays an animation where the Camera moves along a path")]
    
    [Serializable]
    public class ShotTypeAnimation : TShotTypeLook
    {
        [SerializeField] private ShotSystemAnimation m_ShotSystemAnimation;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public ShotSystemAnimation Animation => m_ShotSystemAnimation;

        public override Vector3 Position { get; set; }
        public override Quaternion Rotation { get; set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ShotTypeAnimation()
        {
            this.m_ShotSystemAnimation = new ShotSystemAnimation();
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------

        public override void OnAwake(ShotCamera shotCamera)
        {
            base.OnAwake(shotCamera);

            this.Position = shotCamera.transform.position;
            this.Rotation = shotCamera.transform.rotation;
            
            this.m_ShotSystemAnimation.OnAwake(this);
        }

        public override void OnEnable(TCamera camera)
        {
            base.OnEnable(camera);
            this.m_ShotSystemAnimation.OnEnable(this, camera);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            this.m_ShotSystemAnimation.OnUpdate(this);
        }
        
        // GIZMOS: --------------------------------------------------------------------------------

        public override void DrawGizmos()
        {
            base.DrawGizmos();
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemAnimation.OnDrawGizmos(this);
        }

        public override void DrawGizmosSelected()
        {
            base.DrawGizmosSelected();
            
            if (!Application.isPlaying) return;
            this.m_ShotSystemAnimation.OnDrawGizmosSelected(this);
        }
    }
}