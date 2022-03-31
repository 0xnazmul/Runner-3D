using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the numeric value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable] [HideLabelsInEditor]
    public class SetNumberLocalName : PropertyTypeSetNumber
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueNumber.TYPE_ID);

        public override void Set(double value, Args args)
        {
            this.m_Variable.Set(value);
        }

        public override void Set(double value, GameObject gameObject) => this.m_Variable.Set(value);

        public override double Get(Args args) => (double) this.m_Variable.Get();
        public override double Get(GameObject gameObject) => (double) this.m_Variable.Get();
        
        public static PropertySetNumber Create => new PropertySetNumber(
            new SetNumberLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}