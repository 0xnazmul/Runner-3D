using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the boolean value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetBoolGlobalList : PropertyTypeSetBool
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueBool.TYPE_ID);

        public override void Set(bool value, Args args) => this.m_Variable.Set(value);
        public override void Set(bool value, GameObject gameObject) => this.m_Variable.Set(value);

        public override bool Get(Args args) => (bool) this.m_Variable.Get();
        public override bool Get(GameObject gameObject) => (bool) this.m_Variable.Get();

        public static PropertySetBool Create => new PropertySetBool(
            new SetBoolGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}