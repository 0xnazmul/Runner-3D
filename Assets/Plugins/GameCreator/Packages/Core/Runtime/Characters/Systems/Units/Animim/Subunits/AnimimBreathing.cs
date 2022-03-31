using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class AnimimBreathing : ISubunit<TUnitAnimim>
    {
        private static readonly int K_BREATH_COEFF = Animator.StringToHash("Breath-Coefficient");
        private static readonly int K_BREATH_CYCLE = Animator.StringToHash("Breath-Cycle");
        
        protected const float NOISE_BREATH_SCALE = 0.3f;
        protected const float NOISE_EXERT_SCALE = 0.3f;

        private const float VALUE_RATE = 0.25f;
        private const float VALUE_EXERTION = 0.1f;
        
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField, Range(0f, 1f)] private float m_Exertion = VALUE_EXERTION;
        [SerializeField, Range(0f, 2f)] private float m_Rate = VALUE_RATE;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private AnimFloat m_AnimExertion = new AnimFloat(VALUE_EXERTION, 0.5f);
        private AnimFloat m_AnimRate = new AnimFloat(VALUE_RATE, 0.5f);
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public float Exertion
        {
            get => m_Exertion;
            set => m_Exertion = Mathf.Clamp(value, 0f, 1f);
        }
        
        public float Rate
        {
            get => m_Rate;
            set => m_Rate = Mathf.Clamp(value, 0f, 2f);
        }

        // IMPLEMENTATIONS: -----------------------------------------------------------------------

        public void OnStartup(TUnitAnimim unit, Character character)
        { }

        public void OnDispose(TUnitAnimim unit, Character character)
        { }

        public void OnEnable(TUnitAnimim unit)
        {
            unit.Animator.SetFloat(K_BREATH_CYCLE, Random.value);
        }

        public void OnDisable(TUnitAnimim unit)
        { }

        public void OnUpdate(TUnitAnimim unit)
        {
            float t = unit.Character.Time.Time;
            unit.Animator.SetFloat(K_BREATH_COEFF, this.GetBreathCoefficient(t));
            
            if (unit.Animator.layerCount < TUnitAnimim.LAYER_BREATH) return;
            unit.Animator.SetLayerWeight(TUnitAnimim.LAYER_BREATH, this.GetExertionCoefficient(t));
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private float GetBreathCoefficient(float t)
        {
            this.m_AnimRate.Update(this.m_Rate);

            float noise = Mathf.PerlinNoise(0f, t) * NOISE_BREATH_SCALE;
            return this.m_AnimRate.Current + noise;
        }
        
        private float GetExertionCoefficient(float t)
        {
            this.m_AnimExertion.Update(this.m_Exertion);
            
            float noise = Mathf.PerlinNoise(0f, t) * NOISE_EXERT_SCALE;
            return this.m_AnimExertion.Current + noise;
        }
    }
}