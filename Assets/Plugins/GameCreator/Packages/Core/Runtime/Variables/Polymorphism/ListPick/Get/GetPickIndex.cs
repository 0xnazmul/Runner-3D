using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Variables
{
    [Title("By Index")]
    [Category("By Index")]
    
    [Description("Selects the list element at a specific position")]
    [Image(typeof(IconListIndex), ColorTheme.Type.Yellow)]
    
    [Serializable]
    public class GetPickIndex : IListGetPick
    {
        [SerializeField] private int m_Index = 0;

        public int GetIndex(int count) => this.m_Index;

        public override string ToString() => this.m_Index.ToString();
    }
}