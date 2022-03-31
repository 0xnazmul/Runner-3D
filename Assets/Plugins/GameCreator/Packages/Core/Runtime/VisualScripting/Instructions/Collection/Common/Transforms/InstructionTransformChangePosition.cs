using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Change Position")]
    [Description("Changes the position of a game object over time")]
    
    [Image(typeof(IconVector3), ColorTheme.Type.Yellow)]

    [Category("Transforms/Change Position")]
    
    [Parameter("Position", "The desired position of the game object")]
    [Parameter("Space", "If the transformation occurs in local or world space")]
    [Parameter("Duration", "How long it takes to perform the transition")]
    [Parameter("Easing", "The change rate of the translation over time")]
    [Parameter("Wait to Complete", "Whether to wait until the translation is finished or not")]
    
    [Keywords("Location", "Translate", "Move", "Displace")]
    [Serializable]
    public class InstructionTransformChangePosition : TInstructionTransform
    {
        [SerializeField] private ChangePosition m_Position = new ChangePosition(Vector3.up);
        
        [Space]
        [Space] [SerializeField] private Space m_Space = Space.Self;
        [SerializeField] private Transition m_Transition = new Transition();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Move {this.m_Transform} {this.m_Position}";

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override async Task Run(Args args)
        {
            GameObject gameObject = this.m_Transform.Get(args);
            if (gameObject == null) return;
            
            Vector3 valueSource = this.m_Space switch
            {
                Space.World => gameObject.transform.position,
                Space.Self => gameObject.transform.localPosition,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            Vector3 valueTarget = this.m_Position.Get(valueSource, args);
            
            ITweenInput tween = new TweenInput<Vector3>(
                valueSource,
                valueTarget,
                this.m_Transition.Duration,
                (a, b, t) =>
                {
                    switch (this.m_Space)
                    {
                        case Space.World:
                            gameObject.transform.position = Vector3.LerpUnclamped(a, b, t);
                            break;
                        
                        case Space.Self:
                            gameObject.transform.localPosition = Vector3.LerpUnclamped(a, b, t);
                            break;
                        
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                },
                Tween.GetHash(typeof(Transform), "position"),
                this.m_Transition.EasingType
            );

            Tween.To(gameObject, tween);
            if (this.m_Transition.WaitToComplete) await this.Until(() => tween.IsFinished);
        }
    }
}