using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public class FieldSetLocalName : TFieldSetVariable
    {
        [SerializeField] protected LocalNameVariables m_Variable;
        [SerializeField] protected IdPathString m_Name;

        // CONSTRUCTORS: --------------------------------------------------------------------------

        public FieldSetLocalName(IdString typeID)
        {
            this.m_TypeID = typeID;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public override void Set(object value)
        {
            if (this.m_Variable == null) return;
            this.m_Variable.Set(this.m_Name.String, value);
        }

        public override object Get()
        {
            return this.m_Variable != null ? this.m_Variable.Get(this.m_Name.String) : null;
        }

        public override string ToString()
        {
            return string.Format(
                "{0}{1}",
                this.m_Variable != null ? this.m_Variable.gameObject.name : "(none)",
                string.IsNullOrEmpty(this.m_Name.String) ? string.Empty : $"[{this.m_Name.String}]" 
            );
        }
    }
}