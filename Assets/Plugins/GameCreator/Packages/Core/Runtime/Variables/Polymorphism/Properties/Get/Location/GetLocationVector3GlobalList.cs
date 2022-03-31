using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Vector3 Global List Variable")]
    [Category("Variables/Vector3 Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Vector3 as the position value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationVector3GlobalList : PropertyTypeGetLocation
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueVector3.TYPE_ID);

        public override Location Get(Args args) => new Location(this.m_Variable.Get<Vector3>());
        public override Location Get(GameObject gameObject) => new Location(this.m_Variable.Get<Vector3>());

        public override string String => this.m_Variable.ToString();
    }
}