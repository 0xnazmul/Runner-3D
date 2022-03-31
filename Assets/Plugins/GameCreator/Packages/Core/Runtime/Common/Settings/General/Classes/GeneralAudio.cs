using System;
using UnityEngine.Audio;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public struct GeneralAudio
    {
        public AudioMixerGroup soundEffectsMixer;
        public AudioMixerGroup ambientMixer;
        public AudioMixerGroup speechMixer;
        public AudioMixerGroup userInterfaceMixer;
    }
}