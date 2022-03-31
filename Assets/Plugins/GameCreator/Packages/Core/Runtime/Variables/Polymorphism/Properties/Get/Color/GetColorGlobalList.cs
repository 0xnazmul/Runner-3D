using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Color value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorGlobalList : PropertyTypeGetColor
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueColor.TYPE_ID);

        public override Color Get(Args args) => this.m_Variable.Get<Color>();
        public override Color Get(GameObject gameObject) => this.m_Variable.Get<Color>();

        public override string String => this.m_Variable.ToString();
    }
}