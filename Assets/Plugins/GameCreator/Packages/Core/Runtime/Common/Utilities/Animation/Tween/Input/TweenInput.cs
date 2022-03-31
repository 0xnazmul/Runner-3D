using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public class TweenInput<T> : ITweenInput
    {
        public delegate void Update(T source, T target, float t);

        // MEMBERS: -------------------------------------------------------------------------------

        private readonly float m_StartTime;
        
        private readonly T m_ValueSource;
        private readonly T m_ValueTarget;

        private readonly Easing.Type m_Easing;
        private readonly Update m_Update;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public int Hash { get; }
        public float Duration { get; }

        public bool IsFinished { get; private set; } = false;
        public bool IsComplete { get; private set; } = false;
        public bool IsCanceled { get; private set; } = false;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action<bool> EventFinish;

        // CONSTRUCTOR: ---------------------------------------------------------------------------
        
        public TweenInput(T source, T target, float duration, Update update, int hash, 
            Easing.Type easing = Easing.Type.QuadInOut)
        {
            this.m_StartTime = Time.time;
            
            this.m_ValueSource = source;
            this.m_ValueTarget = target;

            this.m_Easing = easing;
            this.m_Update = update;

            this.Duration = duration;
            this.Hash = hash;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool OnUpdate()
        {
            float elapsed = Time.time - this.m_StartTime;
            float t = this.Duration <= float.Epsilon ? 1f : Mathf.Clamp01(elapsed / this.Duration);
            
            this.m_Update.Invoke(
                this.m_ValueSource, 
                this.m_ValueTarget,
                Easing.GetEase(this.m_Easing, 0f, 1f, t)
            );
            
            return t >= 1f;
        }

        public void OnComplete()
        {
            this.IsFinished = true;
            this.IsComplete = true;
            this.EventFinish?.Invoke(true);
        }
        
        public void OnCancel()
        {
            this.IsFinished = true;
            this.IsCanceled = true;
            this.EventFinish?.Invoke(false);
        }
    }
}