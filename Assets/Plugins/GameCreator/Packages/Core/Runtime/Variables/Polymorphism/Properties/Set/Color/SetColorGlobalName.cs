using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the Color value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetColorGlobalName : PropertyTypeSetColor
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueColor.TYPE_ID);

        public override void Set(Color value, Args args) => this.m_Variable.Set(value);
        public override void Set(Color value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Color Get(Args args) => (Color) this.m_Variable.Get();
        public override Color Get(GameObject gameObject) => (Color) this.m_Variable.Get();
        
        public static PropertySetColor Create => new PropertySetColor(
            new SetColorGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}