using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Category("Cameras/Shots/Orbit/Enable Zoom")]
    [Image(typeof(IconShotThirdPerson), ColorTheme.Type.Yellow)]

    [Version(0, 1, 1)]

    [Title("Enable Zoom")]
    [Description("Toggles the active state of a Camera Shot's Zoom system")]

    [Parameter("Shot", "The camera Third Person Shot's Zoom that becomes active or inactive")]
    [Parameter("Active", "The on/off state of the Shot's Third Person Zoom system")]

    [Keywords("Cameras", "Disable", "Activate", "Deactivate", "Bool", "Toggle", "Off", "On")]

    [Serializable]
    public class InstructionShotThirdPersonEnableZoom : Instruction
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

            ShotTypeThirdPerson shotThirdPerson = shot.ShotType as ShotTypeThirdPerson;
            if (shotThirdPerson == null) return DefaultResult;
            
            shotThirdPerson.Zoom.Active = this.m_Active.Get(args);
            return DefaultResult;
        }
    }
}