using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class AnimimTwitching : ISubunit<TUnitAnimim>
    {
        private static readonly int K_TWITCH_COEFF = Animator.StringToHash("Twitch-Coefficient");
        private static readonly int K_TWITCH_CYCLE = Animator.StringToHash("Twitch-Cycle");
        
        protected const float NOISE_TWITCH_MAGNITUDE = 0.3f;
        protected const float NOISE_TWITCH_FREQUENCY = 5f;

        private const float VALUE_WEIGHT = 0.5f;

        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField, Range(0f, 1f)] private float m_Weight = VALUE_WEIGHT;

        // MEMBERS: -------------------------------------------------------------------------------

        private float m_RandomNoiseX;
        
        private AnimFloat m_AnimWeight = new AnimFloat(VALUE_WEIGHT, 0.5f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public float Weight
        {
            get => m_Weight;
            set => m_Weight = Mathf.Clamp(value, 0f, 1f);
        }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        public void OnStartup(TUnitAnimim unit, Character character)
        {
            this.m_RandomNoiseX = Random.Range(0f, NOISE_TWITCH_FREQUENCY);
        }

        public void OnDispose(TUnitAnimim unit, Character character)
        { }

        public void OnEnable(TUnitAnimim unit)
        {
            unit.Animator.SetFloat(K_TWITCH_CYCLE, Random.value);
        }

        public void OnDisable(TUnitAnimim unit)
        { }

        public void OnUpdate(TUnitAnimim unit)
        {
            float t = unit.Character.Time.Time;
            unit.Animator.SetFloat(K_TWITCH_COEFF, this.GetTwitchCoefficient(t));
            
            if (unit.Animator.layerCount < TUnitAnimim.LAYER_TWITCH) return;
            unit.Animator.SetLayerWeight(TUnitAnimim.LAYER_TWITCH, this.GetWeight());
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private float GetTwitchCoefficient(float t)
        {
            float variation = NOISE_TWITCH_MAGNITUDE * Mathf.PerlinNoise(
                this.m_RandomNoiseX, 
                t / NOISE_TWITCH_FREQUENCY
            );
            
            return 1f + (variation - NOISE_TWITCH_MAGNITUDE / 2f);
        }
        
        private float GetWeight()
        {
            this.m_AnimWeight.Update(this.m_Weight);
            return this.m_AnimWeight.Current;
        }
    }
}