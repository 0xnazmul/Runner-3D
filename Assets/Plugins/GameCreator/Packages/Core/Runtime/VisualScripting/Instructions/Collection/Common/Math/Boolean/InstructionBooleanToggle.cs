using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Toggle Bool")]
    [Description("Toggles the value of a Boolean value")]

    [Category("Math/Boolean/Toggle Bool")]

    [Parameter("Set", "The boolean value that stores the result")]
    [Parameter("From", "The boolean value that is toggled")]

    [Keywords("Change", "Boolean", "Variable", "Not", "Flip", "Switch")]
    [Image(typeof(IconToggleOff), ColorTheme.Type.Red)]
    
    [Serializable]
    public class InstructionBooleanToggle : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] 
        private PropertySetBool m_Set = SetBoolGlobalName.Create;
        
        [SerializeField]
        private PropertyGetBool m_From = GetBoolValue.Create(true);

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public override string Title => $"Set {this.m_Set} = !{this.m_From}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            bool value = this.m_From.Get(args);
            this.m_Set.Set(!value, args);

            return DefaultResult;
        }
    }
}