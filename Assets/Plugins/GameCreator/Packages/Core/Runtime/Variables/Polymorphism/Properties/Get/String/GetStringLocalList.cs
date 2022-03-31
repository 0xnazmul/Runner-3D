using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the string value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringLocalList : PropertyTypeGetString
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueString.TYPE_ID);

        public override string Get(Args args) => this.m_Variable.Get<string>();
        public override string Get(GameObject gameObject) => this.m_Variable.Get<string>();

        public override string String => this.m_Variable.ToString();
    }
}