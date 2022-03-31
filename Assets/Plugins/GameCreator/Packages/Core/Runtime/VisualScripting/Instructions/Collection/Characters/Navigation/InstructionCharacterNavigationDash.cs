using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Dash")]
    [Description("Moves the Character in the chosen direction for a brief period of time")]

    [Category("Characters/Navigation/Dash")]

    [Parameter("Direction", "Vector oriented towards the desired direction")]
    [Parameter("Speed", "Velocity the Character moves throughout the whole movement")]
    [Parameter("Damping", "Defines the duration and gradually changes the rate of the movement over time")]
    [Parameter("Wait to Finish", "If true this Instruction waits until the dash is completed")]
    
    [Parameter("Animation Forward", "Animation played on the Character when dashing forward")]
    [Parameter("Animation Backward", "Animation played on the Character when dashing backwards")]
    [Parameter("Animation Right", "Animation played on the Character when dashing right")]
    [Parameter("Animation Left", "Animation played on the Character when dashing left")]

    [Example(
        "The Damping value defines both the duration and the velocity rate at which the Character " +
        "moves when performing the Dash. To change the duration of the dash open the animation " +
        "curve window and move the last keyframe to the left to decrease the duration or to the " +
        "right to increase it."
    )]
    
    [Example(
        "The Damping value also defines the coefficient rate at which the Character moves " +
        "while performing the Dash. By default the Character starts with a coefficient of 0. " +
        "After 0.2 seconds it increases to 1 and goes back to 0 after 0.8 seconds. This curve " +
        "is evaluated while performing a Dash and the coefficient is extracted from the curve " +
        "and multiplied by the Speed to gradually change the rate at which the Character moves. " +
        "For this reason, it is recommended that the Damping stay between 0 and 1."
    )]
    
    [Keywords("Leap", "Blink", "Roll", "Flash")]
    [Image(typeof(IconCharacterDash), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionCharacterNavigationDash : TInstructionCharacterNavigation
    {
        private const float TRANSITION_IN = 0.1f;
        private const float TRANSITION_OUT = 0.25f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------

        [SerializeField] private PropertyGetDirection m_Direction = GetDirectionCharactersMoving.Create;
        
        [Space]
        [SerializeField] private PropertyGetDecimal m_Speed = new PropertyGetDecimal(10f);
        [SerializeField] private AnimationCurve m_Damping = new AnimationCurve(
            new Keyframe(0.0f, 0f, 0f, 0f), 
            new Keyframe(0.2f, 1f, 0f, 0f),
            new Keyframe(1.0f, 0f, 0f, 0f)
        );
        
        [Space]
        [SerializeField] private bool m_WaitToFinish = true;

        [Space] 
        [SerializeField] private AnimationClip m_AnimationForward;
        [SerializeField] private AnimationClip m_AnimationBackward;
        [SerializeField] private AnimationClip m_AnimationRight;
        [SerializeField] private AnimationClip m_AnimationLeft;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Dash {this.m_Character} towards {this.m_Direction}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override async Task Run(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;
            if (character.Busy.AreLegsBusy) return;

            Vector3 direction = this.m_Direction.Get(args);
            if (direction == Vector3.zero) direction = character.transform.forward;
            
            float speed = (float) this.m_Speed.Get(args);
            float duration = this.m_Damping.length > 0
                ? this.m_Damping[this.m_Damping.length - 1].time
                : 0f;

            ITweenInput tween = new TweenInput<float>(
                0f, 1f, duration,
                (a, b, t) =>
                {
                    character.Motion.MoveToDirection(
                        direction.normalized * (this.m_Damping.Evaluate(t) * speed), 
                        Space.World
                    );
                },
                Tween.GetHash(typeof(Transform), "position"),
                Easing.Type.Linear
            );

            tween.EventFinish += isComplete =>
            {
                character.Busy.RemoveLegsBusy();
                character.Motion.StopToDirection();
            };
            
            character.Busy.MakeLegsBusy();
            float angle = Vector3.SignedAngle(
                direction, 
                character.transform.forward, 
                Vector3.up
            );
            
            AnimationClip animationClip = this.GetAnimationClip(angle);
            if (animationClip != null)
            {
                ConfigGesture config = new ConfigGesture(
                    0f, animationClip.length, 1f, false,
                    TRANSITION_IN, TRANSITION_OUT
                );
                
                _ = character.Gestures.CrossFade(animationClip, null, BlendMode.Blend, config, true);
            }

            Tween.To(character.gameObject, tween);
            if (this.m_WaitToFinish) await this.Until(() => tween.IsFinished);
        }

        private AnimationClip GetAnimationClip(float angle)
        {
            if (angle <= 45f  && angle >= -45f) return this.m_AnimationForward;
            if (angle < 135f  && angle > 45f)   return this.m_AnimationLeft;
            if (angle > -135f && angle < -45f)  return this.m_AnimationRight;
            
            return m_AnimationBackward;
        }
    }
}