using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Wait Seconds")]
    [Description("Waits a certain amount of seconds")]

    [Category("Time/Wait Seconds")]

    [Parameter(
        "Seconds",
        "The amount of seconds to wait"
    )]

    [Keywords("Wait", "Time", "Seconds", "Minutes", "Cooldown", "Timeout", "Yield")]
    [Image(typeof(IconTimer), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class InstructionCommonTimeWait : Instruction
    {
        [SerializeField]
        private PropertyGetDecimal m_Seconds = new PropertyGetDecimal(1f);

        public override string Title =>
            $"Wait {this.m_Seconds} {(this.m_Seconds.ToString() == "1" ? "second" : "seconds")}";

        protected override async Task Run(Args args)
        {
            float value = (float) this.m_Seconds.Get(args);
            await this.Time(value);
        }
    }
}
