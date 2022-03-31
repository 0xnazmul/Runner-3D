using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Category("Cameras/Shots/Lock On/Enable Zoom")]
    [Image(typeof(IconShotLockOn), ColorTheme.Type.Yellow)]

    [Version(0, 1, 1)]

    [Title("Enable Zoom")]
    [Description("Toggles the active state of a Camera Shot's Zoom system")]

    [Parameter("Shot", "The camera Lock On Shot that becomes active or inactive")]
    [Parameter("Active", "The on/off state of the Shot's Lock On Zoom system")]

    [Keywords("Cameras", "Disable", "Activate", "Deactivate", "Bool", "Toggle", "Off", "On")]

    [Serializable]
    public class InstructionShotLockOnEnableZoom : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetShot m_Shot = GetShotInstance.Create;

        [Space]
        [SerializeField] private PropertyGetBool m_Active = new PropertyGetBool(true);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Shot} Zoom to {this.m_Active}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            ShotCamera shot = this.m_Shot.Get(args);
            if (shot == null) return DefaultResult;

            ShotTypeLockOn shotLockOn = shot.ShotType as ShotTypeLockOn;
            if (shotLockOn == null) return DefaultResult;
            
            shotLockOn.Zoom.Active = this.m_Active.Get(args);
            return DefaultResult;
        }
    }
}