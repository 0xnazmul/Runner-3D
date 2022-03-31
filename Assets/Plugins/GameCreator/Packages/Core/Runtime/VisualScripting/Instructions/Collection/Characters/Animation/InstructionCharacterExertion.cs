using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Change Exertion")]
    [Description("Changes the Exertion value of a Character over time")]

    [Category("Characters/Animation/Change Exertion")]
    
    [Parameter("Exertion", "The target Exertion value between 0 and 1. Default is 0.25")]
    [Parameter("Duration", "How long it takes to perform the transition")]
    [Parameter("Easing", "The change rate of the parameter over time")]
    [Parameter("Wait to Complete", "Whether to wait until the transition finishes")]
    
    [Example(
        "The Heart Rate value goes hand in hand with the Exertion. The Heart Rate controls " +
        "the speed that the breathing animation plays. The Exertion controls the magnitude of " +
        "the breathing animation."
    )]

    [Keywords("Tire", "Effort", "Struggle", "Sweat", "Exercise")]
    [Image(typeof(IconExertion), ColorTheme.Type.Yellow)]

    [Serializable]
    public class InstructionCharacterExertion : TInstructionCharacterProperty
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private ChangeDecimal m_Exertion = new ChangeDecimal(0.25f);
        [SerializeField] private Transition m_Transition = new Transition();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Change Exertion of {this.m_Character} {this.m_Exertion}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override async Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            float valueSource = character.Animim.Exertion;
            float valueTarget = (float) this.m_Exertion.Get(valueSource, args);

            ITweenInput tween = new TweenInput<float>(
                valueSource,
                valueTarget,
                this.m_Transition.Duration,
                (a, b, t) => character.Animim.Exertion = Mathf.Lerp(a, b, t),
                Tween.GetHash(typeof(AnimimBreathing), "exertion"),
                this.m_Transition.EasingType
            );
            
            Tween.To(character.gameObject, tween);
            if (this.m_Transition.WaitToComplete) await this.Until(() => tween.IsFinished);
        }
    }
}