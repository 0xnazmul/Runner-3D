using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the string value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetStringLocalList : PropertyTypeSetString
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueString.TYPE_ID);

        public override void Set(string value, Args args) => this.m_Variable.Set(value);
        public override void Set(string value, GameObject gameObject) => this.m_Variable.Set(value);

        public override string Get(Args args) => this.m_Variable.Get().ToString();
        public override string Get(GameObject gameObject) => this.m_Variable.Get().ToString();
        
        public static PropertySetString Create => new PropertySetString(
            new SetStringLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}