using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Playables;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Play Animation Clip")]
    [Description("Plays an Animation Clip on the chosen Animator")]
    
    [Image(typeof(IconPlayCircle), ColorTheme.Type.Blue)]

    [Category("Animator/Play Animation Clip")]
    
    [Parameter("Animation Clip", "The Animation Clip that is played")]

    [Keywords("Animate", "Reproduce", "Sequence", "Cinematic")]
    [Serializable]
    public class InstructionAnimatorPlayAnimation : TInstructionAnimator
    {
        [SerializeField] private AnimationClip m_AnimationClip;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Play {0} on {1}",
            this.m_AnimationClip != null ? this.m_AnimationClip.name : "(none)",
            this.m_Animator
        );

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override Task Run(Args args)
        {
            GameObject gameObject = this.m_Animator.Get(args);
            if (gameObject == null) return DefaultResult;

            Animator animator = gameObject.Get<Animator>();
            if (animator == null) return DefaultResult;

            AnimationPlayableUtilities.PlayClip(animator, this.m_AnimationClip, out PlayableGraph graph);
            return DefaultResult;
        }
    }
}