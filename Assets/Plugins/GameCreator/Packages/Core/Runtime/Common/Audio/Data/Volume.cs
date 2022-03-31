using System;
using UnityEngine;

namespace GameCreator.Runtime.Common.Audio
{
    [Serializable]
    public class Volume
    {
        private const float DEFAULT_VALUE = 1f;
        private const float SMOOTH = 0.25f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private float m_Master  = DEFAULT_VALUE;
        [SerializeField] private float m_SFX     = DEFAULT_VALUE;
        [SerializeField] private float m_Ambient = DEFAULT_VALUE;
        [SerializeField] private float m_Speech  = DEFAULT_VALUE;
        [SerializeField] private float m_UI      = DEFAULT_VALUE;

        // PRIVATE PROPERTIES: --------------------------------------------------------------------
        
        private AnimFloat ValueMaster  { get; set; } = new AnimFloat(DEFAULT_VALUE, SMOOTH);
        private AnimFloat ValueSFX     { get; set; } = new AnimFloat(DEFAULT_VALUE, SMOOTH);
        private AnimFloat ValueAmbient { get; set; } = new AnimFloat(DEFAULT_VALUE, SMOOTH);
        private AnimFloat ValueSpeech  { get; set; } = new AnimFloat(DEFAULT_VALUE, SMOOTH);
        private AnimFloat ValueUI      { get; set; } = new AnimFloat(DEFAULT_VALUE, SMOOTH);

        // PUBLIC PROPERTIES: ---------------------------------------------------------------------

        public float Master
        {
            get => Mathf.Clamp01(this.ValueMaster.Current);
            set => this.m_Master = Mathf.Clamp01(value);
        }
        
        public float SoundEffects
        {
            get => Mathf.Clamp01(this.ValueSFX.Current);
            set => this.m_SFX = value;
        }
        
        public float Ambient
        {
            get => Mathf.Clamp01(this.ValueAmbient.Current);
            set => this.m_Ambient = value;
        }
        
        public float Speech
        {
            get => Mathf.Clamp01(this.ValueSpeech.Current);
            set => this.m_Speech = value;
        }
        
        public float UI
        {
            get => Mathf.Clamp01(this.ValueUI.Current);
            set => this.m_UI = value;
        }

        // UPDATE METHODS: ------------------------------------------------------------------------

        internal void Update()
        {
            float deltaTime = Time.unscaledDeltaTime;
            
            this.ValueMaster.UpdateWithDelta(this.m_Master, deltaTime);
            this.ValueSFX.UpdateWithDelta(this.m_SFX, deltaTime);
            this.ValueAmbient.UpdateWithDelta(this.m_Ambient, deltaTime);
            this.ValueSpeech.UpdateWithDelta(this.m_Speech, deltaTime);
            this.ValueUI.UpdateWithDelta(this.m_UI, deltaTime);
        }
    }
}