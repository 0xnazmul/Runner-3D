using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the numeric value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetNumberLocalList : PropertyTypeSetNumber
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueNumber.TYPE_ID);

        public override void Set(double value, Args args) => this.m_Variable.Set(value);
        public override void Set(double value, GameObject gameObject) => this.m_Variable.Set(value);

        public override double Get(Args args) => (double) this.m_Variable.Get();
        public override double Get(GameObject gameObject) => (double) this.m_Variable.Get();
        
        public static PropertySetNumber Create => new PropertySetNumber(
            new SetNumberLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}