using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class Transition
    {
        [SerializeField] private float m_Duration = 0f;
        [SerializeField] private Easing.Type m_Easing = Easing.Type.QuadInOut;
        [SerializeField] private bool m_WaitToComplete = true;

        // PROPERTIES: ----------------------------------------------------------------------------

        public float Duration => this.m_Duration;
        
        public Easing.Type EasingType => this.m_Easing;

        public bool WaitToComplete => m_WaitToComplete;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Transition()
        { }

        public Transition(float duration, Easing.Type easing, bool waitToComplete)
        {
            this.m_Duration = duration;
            this.m_Easing = easing;
            this.m_WaitToComplete = waitToComplete;
        }
    }
}