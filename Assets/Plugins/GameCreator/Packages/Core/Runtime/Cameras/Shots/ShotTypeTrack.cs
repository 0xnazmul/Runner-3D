using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Follow Track")]
    [Category("Follow Track")]
    [Image(typeof(IconShotTrack), ColorTheme.Type.Blue)]
    
    [Description("Follows the target from along a pre-defined path segment")]
    
    [Serializable]
    public class ShotTypeTrack : TShotTypeLook
    {
        [SerializeField] private ShotSystemTrack m_ShotSystemTrack;
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        public ShotSystemTrack Track => m_ShotSystemTrack;
        
        public override Vector3 Position { get; set; }
        public override Quaternion Rotation { get; set; }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public ShotTypeTrack()
        {
            this.m_ShotSystemTrack = new ShotSystemTrack();
        }
        
        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            this.m_ShotSystemTrack.OnUpdate(this);
        }
    }
}