using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the boolean value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetBoolGlobalList : PropertyTypeGetBool
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueBool.TYPE_ID);

        public override bool Get(Args args) => this.m_Variable.Get<bool>();
        public override bool Get(GameObject gameObject) => this.m_Variable.Get<bool>();

        public override string String => this.m_Variable.ToString();
    }
}