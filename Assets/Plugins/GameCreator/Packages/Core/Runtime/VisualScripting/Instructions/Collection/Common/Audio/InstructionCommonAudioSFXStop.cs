using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Stop Sound Effect")]
    [Description("Stops a currently playing Sound Effect")]

    [Category("Audio/Stop Sound Effect")]

    [Keywords("Audio", "Sounds", "Silence", "Fade", "Mute")]
    [Image(typeof(IconMusicNote), ColorTheme.Type.TextLight, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionCommonAudioSFXStop : Instruction
    {
        [SerializeField] private AudioClip m_AudioClip = null;
        [SerializeField] private bool m_WaitToComplete = false;
        [SerializeField] private float transitionOut = 0.1f;

        public override string Title => string.Format(
            "Stop SFX: {0} {1}",
            this.m_AudioClip != null ? this.m_AudioClip.name : "(none)",
            this.transitionOut < float.Epsilon 
                ? string.Empty 
                : string.Format(
                    "in {0} second{1}", 
                    this.transitionOut,
                    Mathf.Approximately(this.transitionOut, 1f) ? string.Empty : "s"
                )
        );

        protected override async Task Run(Args args)
        {
            if (this.m_WaitToComplete)
            {
                await AudioManager.Instance.SoundEffect.Stop(
                    this.m_AudioClip,
                    this.transitionOut
                );
            }
            else
            {
                _ = AudioManager.Instance.SoundEffect.Stop(
                    this.m_AudioClip,
                    this.transitionOut
                );
            }
        }
    }
}