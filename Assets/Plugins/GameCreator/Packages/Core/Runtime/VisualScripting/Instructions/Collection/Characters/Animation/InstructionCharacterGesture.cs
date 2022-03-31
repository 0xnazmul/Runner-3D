using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;
using GameCreator.Runtime.Characters;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Play Gesture")]
    [Description("Plays an Animation Clip on a Character once")]

    [Category("Characters/Animation/Play Gesture")]

    [Parameter("Character", "The character that plays the animation")]
    [Parameter("Animation Clip", "The Animation Clip that is played")]
    [Parameter(
        "Avatar Mask",
         "(Optional) Allows to play the animation on specific body parts of the Character"
    )]
    [Parameter(
        "Blend Mode",
        "Additively adds the new animation on top of the rest or overrides any lower layer animations"
    )]
    
    [Parameter("Delay", "Amount of seconds to wait before the animation starts to play")]
    [Parameter("Speed", "Speed coefficient at which the animation plays. 1 means normal speed")]
    [Parameter("Transition In", "The amount of seconds the animation takes to blend in")]
    [Parameter("Transition Out", "The amount of seconds the animation takes to blend out")]
    
    [Parameter("Wait To Complete", "If true this Instruction waits until the animation is complete")]

    [Keywords("Characters", "Animation", "Animate", "Gesture", "Play")]
    [Image(typeof(IconCharacterGesture), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionCharacterGesture : Instruction
    {
        [SerializeField] private PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();

        [Space]
        [SerializeField] private AnimationClip m_AnimationClip = null;
        [SerializeField] private AvatarMask m_AvatarMask = null;
        [SerializeField] private BlendMode m_BlendMode = BlendMode.Blend;

        [Space] 
        [SerializeField] private float m_Delay = 0f;
        [SerializeField] private float m_Speed = 1f;
        [SerializeField] private bool m_UseRootMotion = false;
        [SerializeField] private float m_TransitionIn = 0.1f;
        [SerializeField] private float m_TransitionOut = 0.1f;

        [Space] 
        [SerializeField] private bool m_WaitToComplete = true;

        public override string Title => string.Format("Gesture {0} on {1}",
            this.m_AnimationClip != null ? this.m_AnimationClip.name : "(none)",
            this.m_Character
        );

        protected override async Task Run(Args args)
        {
            if (this.m_AnimationClip == null) return;
            
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return;
            
            ConfigGesture configuration = new ConfigGesture(
                this.m_Delay, this.m_AnimationClip.length / this.m_Speed, 
                this.m_Speed, this.m_UseRootMotion,
                this.m_TransitionIn, this.m_TransitionOut
            );
            
            Task gestureTask = character.Gestures.CrossFade(
                this.m_AnimationClip, this.m_AvatarMask, this.m_BlendMode,
                configuration, false
            );

            if (this.m_WaitToComplete) await gestureTask;
        }
    }
}