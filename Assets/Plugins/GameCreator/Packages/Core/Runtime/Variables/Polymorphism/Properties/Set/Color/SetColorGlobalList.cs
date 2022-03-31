using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the Color value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetColorGlobalList : PropertyTypeSetColor
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueColor.TYPE_ID);

        public override void Set(Color value, Args args) => this.m_Variable.Set(value);
        public override void Set(Color value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Color Get(Args args) => (Color) this.m_Variable.Get();
        public override Color Get(GameObject gameObject) => (Color) this.m_Variable.Get();
        
        public static PropertySetColor Create => new PropertySetColor(
            new SetColorGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}