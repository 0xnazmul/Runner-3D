using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Serializable]
    public abstract class TList<T> : TPolymorphicList<T> where T : TVariable
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeReference] private T[] m_Source = Array.Empty<T>(); 
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override int Length => this.m_Source.Length;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------

        protected TList()
        { }

        protected TList(params T[] variables) : this()
        {
            this.m_Source = variables;
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public T Get(int index)
        {
            return this.m_Source[index];
        }
    }
}