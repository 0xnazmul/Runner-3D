using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the boolean value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable] [HideLabelsInEditor]
    public class SetBoolLocalName : PropertyTypeSetBool
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueBool.TYPE_ID);

        public override void Set(bool value, Args args) => this.m_Variable.Set(value);
        public override void Set(bool value, GameObject gameObject) => this.m_Variable.Set(value);

        public override bool Get(Args args) => (bool) this.m_Variable.Get();
        public override bool Get(GameObject gameObject) => (bool) this.m_Variable.Get();
        
        public static PropertySetBool Create => new PropertySetBool(
            new SetBoolLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}