using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public abstract class TFieldGetVariable
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] protected IdString m_TypeID = ValueNull.TYPE_ID;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public T Get<T>()
        {
            object value = this.Get();
            return value is T typedValue ? typedValue : default;
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        public abstract object Get();
        public abstract override string ToString();
    }
}