using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class NavAreaValue
    {
        [SerializeField] private int m_Index;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Index => this.m_Index;

        // OVERRIDES: -----------------------------------------------------------------------------

        public override string ToString()
        {
            if (this.m_Index < 0 || this.m_Index > 31) return "(unknown)";
            string value = "(unnamed)";
            
            #if UNITY_EDITOR

            string[] areaNames = UnityEditor.GameObjectUtility.GetNavMeshAreaNames();
            value = areaNames[this.m_Index];
            
            #endif
            
            return !string.IsNullOrEmpty(value) ? value : "(unnamed)";
        }
    }
}