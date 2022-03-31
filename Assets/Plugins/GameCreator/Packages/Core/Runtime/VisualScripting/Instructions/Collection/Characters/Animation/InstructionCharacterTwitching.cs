using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Change Twitching")]
    [Description("Changes the magnitude of the subtle and random movement applied to each Character's bone")]

    [Category("Characters/Animation/Change Twitching")]
    
    [Parameter("Twitching", "The target Twitching value between 0 and 1. Default is 1")]
    [Parameter("Duration", "How long it takes to perform the transition")]
    [Parameter("Easing", "The change rate of the parameter over time")]
    [Parameter("Wait to Complete", "Whether to wait until the transition is finished")]

    [Example(
        "The Twitching value allows a Character to express subtle random movement found in " +
        "life beings. Paired with the Breathing animation, it allows to have a consistent " +
        "rhythm even when blending between other animations. It can also be useful to create " +
        "idle animations using a static pose."
    )]
    
    [Keywords("Tire", "Effort", "Struggle", "Sweat", "Exercise")]
    [Image(typeof(IconTwitching), ColorTheme.Type.Yellow)]

    [Serializable]
    public class InstructionCharacterTwitching : TInstructionCharacterProperty
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private ChangeDecimal m_Twitching = new ChangeDecimal(1f);
        [SerializeField] private Transition m_Transition = new Transition();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Change Twitching of {this.m_Character} {this.m_Twitching}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override async Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;

            float valueSource = character.Animim.Twitching;
            float valueTarget = (float) this.m_Twitching.Get(valueSource, args);

            ITweenInput tween = new TweenInput<float>(
                valueSource,
                valueTarget,
                this.m_Transition.Duration,
                (a, b, t) => character.Animim.Twitching = Mathf.Lerp(a, b, t),
                Tween.GetHash(typeof(AnimimTwitching), "weight"),
                this.m_Transition.EasingType
            );
            
            Tween.To(character.gameObject, tween);
            if (this.m_Transition.WaitToComplete) await this.Until(() => tween.IsFinished);
        }
    }
}