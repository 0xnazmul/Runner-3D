using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Version(0, 1, 1)]

    [Title("Play Speech")]
    [Description("Plays an Audio Clip speech over just once")]

    [Category("Audio/Play Speech")]

    [Parameter("Audio Clip", "The Audio Clip to be played")]
    [Parameter("Wait To Complete", "Check if you want to wait until the sound finishes")]
    [Parameter("Spatial Blending", "Whether the sound is placed in a 3D space or not")]
    [Parameter("Target", "A Game Object reference that the sound follows as its source")]
    
    [Keywords("Audio", "Voice", "Voices", "Sounds", "Character")]
    [Image(typeof(IconFace), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class InstructionCommonAudioSpeechPlay : Instruction
    {
        [SerializeField] private AudioClip m_AudioClip = null;
        [SerializeField] private bool m_WaitToComplete = false;
        
        [SerializeField] private AudioConfigSpeech m_Config = new AudioConfigSpeech();

        public override string Title => string.Format(
            "Play Speech: {0}",
            this.m_AudioClip != null ? this.m_AudioClip.name : "(none)"
        );

        protected override async Task Run(Args args)
        {
            if (this.m_WaitToComplete)
            {
                await AudioManager.Instance.Speech.Play(
                    this.m_AudioClip, 
                    this.m_Config,
                    args
                );
            }
            else
            {
                _ = AudioManager.Instance.Speech.Play(
                    this.m_AudioClip, 
                    this.m_Config,
                    args
                );
            }
        }
    }
}