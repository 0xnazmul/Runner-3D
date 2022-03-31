using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [Serializable]
    public class ConditionList : TPolymorphicList<Condition>
    {
        [SerializeReference]
        private Condition[] m_Conditions = Array.Empty<Condition>();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Conditions.Length;

        // EVENTS: --------------------------------------------------------------------------------

        public event Action EventStartCheck;
        public event Action EventEndCheck;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public ConditionList()
        { }

        public ConditionList(params Condition[] conditions) : this()
        {
            this.m_Conditions = conditions;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public bool Check(Args args)
        {
            this.EventStartCheck?.Invoke();

            foreach (Condition condition in this.m_Conditions)
            {
                if (condition == null) continue;
                
                if (!condition.Check(args))
                {
                    this.EventEndCheck?.Invoke();
                    return false;
                }
            }

            this.EventEndCheck?.Invoke();
            return true;
        }

        public Condition Get(int index)
        {
            index = Mathf.Clamp(index, 0, this.Length - 1);
            return this.m_Conditions[index];
        }
    }
}
