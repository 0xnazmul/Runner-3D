using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Vector3 Global Name Variable")]
    [Category("Variables/Vector3 Global Name Variable")]
    
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]
    [Description("Returns the Vector3 as the position value of a Global Name Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationVector3GlobalName : PropertyTypeGetLocation
    {
        [SerializeField]
        protected FieldGetGlobalName m_Variable = new FieldGetGlobalName(ValueVector3.TYPE_ID);

        public override Location Get(Args args) => new Location(this.m_Variable.Get<Vector3>());
        public override Location Get(GameObject gameObject) => new Location(this.m_Variable.Get<Vector3>());

        public override string String => this.m_Variable.ToString();
    }
}