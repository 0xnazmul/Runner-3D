using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Stop Ambient")]
    [Description("Stops a currently playing Ambient audio")]

    [Category("Audio/Stop Ambient")]
    
    [Parameter("Audio Clip", "The Audio Clip to be played")]
    [Parameter("Wait To Complete", "Check if you want to wait until the sound has faded out")]
    [Parameter("Transition Out", "Time it takes for the sound to fade out")]

    [Keywords("Audio", "Music", "Ambience", "Background", "Fade", "Mute")]
    [Image(typeof(IconHeadset), ColorTheme.Type.TextLight, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionCommonAudioAmbientStop : Instruction
    {
        [SerializeField] private AudioClip m_AudioClip = null;
        [SerializeField] private bool m_WaitToComplete = false;
        [SerializeField] private float transitionOut = 2f;

        public override string Title => string.Format(
            "Stop Ambient: {0} {1}",
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
                await AudioManager.Instance.Ambient.Stop(
                    this.m_AudioClip,
                    this.transitionOut
                );
            }
            else
            {
                _ = AudioManager.Instance.Ambient.Stop(
                    this.m_AudioClip,
                    this.transitionOut
                );
            }
        }
    }
}