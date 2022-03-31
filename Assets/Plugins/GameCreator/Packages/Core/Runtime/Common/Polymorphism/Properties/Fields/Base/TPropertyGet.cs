using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public abstract class TPropertyGet<TType, TValue> 
        : IProperty where TType : TPropertyTypeGet<TValue>
    {
        [SerializeReference]
        protected TType m_Property;

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TPropertyGet(TType defaultType)
        {
            this.m_Property = defaultType;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public virtual TValue Get(Args args) => this.m_Property.Get(args);
        public virtual TValue Get(GameObject target) => this.m_Property.Get(target);

        public virtual TValue Get(Component component)
        {
            return this.Get(component ? component.gameObject : null);
        }

        public override string ToString()
        {
            return this.m_Property?.String ?? "(none)";
        }
    }
}