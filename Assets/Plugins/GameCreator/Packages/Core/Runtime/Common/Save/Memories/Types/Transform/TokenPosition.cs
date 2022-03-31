using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class TokenPosition : Token
    {
        [SerializeField]
        private Vector3 m_Position;

        // PROPERTIES: ----------------------------------------------------------------------------

        public Vector3 Position => this.m_Position;
        
        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public TokenPosition(GameObject target) : base()
        {
            this.m_Position = target.transform.position;
        }
    }
}