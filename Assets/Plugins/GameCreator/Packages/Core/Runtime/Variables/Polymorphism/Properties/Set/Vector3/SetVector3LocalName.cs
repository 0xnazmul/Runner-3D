using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the Vector3 value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable] [HideLabelsInEditor]
    public class SetVector3LocalName : PropertyTypeSetVector3
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueVector3.TYPE_ID);

        public override void Set(Vector3 value, Args args) => this.m_Variable.Set(value);
        public override void Set(Vector3 value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Vector3 Get(Args args) => (Vector3) this.m_Variable.Get();
        public override Vector3 Get(GameObject gameObject) => (Vector3) this.m_Variable.Get();
        
        public static PropertySetVector3 Create => new PropertySetVector3(
            new SetVector3LocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}