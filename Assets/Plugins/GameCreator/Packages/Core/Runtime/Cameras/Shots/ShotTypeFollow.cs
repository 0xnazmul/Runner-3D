using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Follow Target")]
    [Category("Follow Target")]
    [Image(typeof(IconShotFollow), ColorTheme.Type.Blue)]
    
    [Description("Follows the target from a certain distance")]
    
    [Serializable]
    public class ShotTypeFollow : TShotTypeLook
    {
        [SerializeField] private ShotSystemFollow m_ShotSystemFollow;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public ShotSystemFollow Follow => m_ShotSystemFollow;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ShotTypeFollow()
        {
            this.m_ShotSystemFollow = new ShotSystemFollow();
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            this.m_ShotSystemFollow.OnUpdate(this);
        }
    }
}