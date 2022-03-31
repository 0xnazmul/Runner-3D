using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Position Z")]
    [Category("Transforms/Position Z")]
    
    [Image(typeof(IconVector3), ColorTheme.Type.Blue)]
    [Description("The Z component of a Vector3 that represents a position in space")]

    [Keywords("Position", "Vector3", "Forward", "Backward")]
    
    [Serializable] [HideLabelsInEditor]
    public class GetDecimalTransformsPositionZ : PropertyTypeGetDecimal
    {
        [SerializeField]
        protected PropertyGetPosition m_Position = GetPositionCharactersPlayer.Create;

        public override double Get(Args args) => this.m_Position.Get(args).z;
        public override double Get(GameObject gameObject) => this.m_Position.Get(gameObject).z;

        public override string String => $"{this.m_Position}.Z";
    }
}