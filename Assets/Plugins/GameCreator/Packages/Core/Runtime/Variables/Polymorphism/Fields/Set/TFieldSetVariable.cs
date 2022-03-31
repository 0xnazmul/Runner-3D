using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public abstract class TFieldSetVariable
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] protected IdString m_TypeID = ValueNull.TYPE_ID;

        // ABSTRACT METHODS: ----------------------------------------------------------------------
        
        public abstract void Set(object value);
        public abstract object Get();

        public abstract override string ToString();
    }
}