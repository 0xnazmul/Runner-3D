using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Enable Look")]
    [Description("Toggles the active state of a Camera Shot's Look system")]

    [Parameter("Shot", "The camera Shot that becomes active or inactive")]
    [Parameter("Active", "The on/off state of the Shot's Look system")]

    [Keywords("Cameras", "Disable", "Activate", "Deactivate", "Bool", "Toggle", "Off", "On")]

    [Serializable]
    public abstract class TInstructionShotEnableLook : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetShot m_Shot = GetShotInstance.Create;

        [Space]
        [SerializeField] private PropertyGetBool m_Active = new PropertyGetBool(true);

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Set {this.m_Shot} Look to {this.m_Active}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            ShotCamera shot = this.m_Shot.Get(args);
            if (shot == null) return DefaultResult;

            TShotTypeLook shotLook = shot.ShotType as TShotTypeLook;
            if (shotLook == null) return DefaultResult;
            
            shotLook.Look.Active = this.m_Active.Get(args);
            return DefaultResult;
        }
    }
}