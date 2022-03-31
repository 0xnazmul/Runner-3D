using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Change Heart Rate")]
    [Description("Changes the Heart Rate value of a Character over time")]

    [Category("Characters/Animation/Change Heart Rate")]
    
    [Parameter("Heart Rate", "The target Heart Rate value between 0 and 2. Default is 1")]
    [Parameter("Duration", "How long it takes to perform the transition")]
    [Parameter("Easing", "The change rate of the parameter over time")]
    [Parameter("Wait to Complete", "Whether to wait until the transition finishes")]
    
    [Example(
        "The Heart Rate value goes hand in hand with the Exertion. The Heart Rate controls " +
        "the speed that the breathing animation plays. The Exertion controls the magnitude of " +
        "the breathing animation."
    )]

    [Keywords("Breathe", "Pump", "Beat", "Pulse")]
    [Image(typeof(IconHeartBeat), ColorTheme.Type.Yellow)]

    [Serializable]
    public class InstructionCharacterHeartRate : TInstructionCharacterProperty
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private ChangeDecimal m_HeartRate = new ChangeDecimal(1f);
        [SerializeField] private Transition m_Transition = new Transition();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Change Heart Rate of {this.m_Character} {this.m_HeartRate}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override async Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            float valueSource = character.Animim.HeartRate;
            float valueTarget = (float) this.m_HeartRate.Get(valueSource, args);

            ITweenInput tween = new TweenInput<float>(
                valueSource,
                valueTarget,
                this.m_Transition.Duration,
                (a, b, t) => character.Animim.HeartRate = Mathf.Lerp(a, b, t),
                Tween.GetHash(typeof(AnimimBreathing), "heart-rate"),
                this.m_Transition.EasingType
            );
            
            Tween.To(character.gameObject, tween);
            if (this.m_Transition.WaitToComplete) await this.Until(() => tween.IsFinished);
        }
    }
}