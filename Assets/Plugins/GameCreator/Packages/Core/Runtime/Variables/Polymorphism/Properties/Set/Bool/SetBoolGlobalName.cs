using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the boolean value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetBoolGlobalName : PropertyTypeSetBool
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueBool.TYPE_ID);

        public override void Set(bool value, Args args) => this.m_Variable.Set(value);
        public override void Set(bool value, GameObject gameObject) => this.m_Variable.Set(value);

        public override bool Get(Args args) => (bool) this.m_Variable.Get();
        public override bool Get(GameObject gameObject) => (bool) this.m_Variable.Get();
        
        public static PropertySetBool Create => new PropertySetBool(
            new SetBoolGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}