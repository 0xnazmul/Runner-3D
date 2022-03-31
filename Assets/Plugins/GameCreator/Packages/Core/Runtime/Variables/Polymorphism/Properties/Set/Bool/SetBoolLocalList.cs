using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the boolean value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetBoolLocalList : PropertyTypeSetBool
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueBool.TYPE_ID);

        public override void Set(bool value, Args args) => this.m_Variable.Set(value);
        public override void Set(bool value, GameObject gameObject) => this.m_Variable.Set(value);

        public override bool Get(Args args) => (bool) this.m_Variable.Get();
        public override bool Get(GameObject gameObject) => (bool) this.m_Variable.Get();
        
        public static PropertySetBool Create => new PropertySetBool(
            new SetBoolLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}