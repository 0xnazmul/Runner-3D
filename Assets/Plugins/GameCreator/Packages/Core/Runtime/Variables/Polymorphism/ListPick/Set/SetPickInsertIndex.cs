using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Title("Insert at Index")]
    [Category("Insert at Index")]
    
    [Description("Inserts a new element at the specified list position")]
    [Image(typeof(IconListIndex), ColorTheme.Type.Blue)]
    
    [Serializable]
    public class SetPickInsertIndex : IListSetPick
    {
        [SerializeField] private int m_Index = 0;

        public int GetIndex(ListVariableRuntime list, int count)
        {
            list.Insert(this.m_Index, default);
            return this.m_Index;
        }
        
        public int GetIndex(int count) => -1;

        public override string ToString() => this.m_Index.ToString();
    }
}