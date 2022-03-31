using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local Name Variable")]
    [Category("Variables/Local Name Variable")]
    
    [Description("Sets the Game Object value of a Local Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple)]

    [Serializable] [HideLabelsInEditor]
    public class SetGameObjectLocalName : PropertyTypeSetGameObject
    {
        [SerializeField]
        protected FieldSetLocalName m_Variable = new FieldSetLocalName(ValueGameObject.TYPE_ID);

        public override void Set(GameObject value, Args args) => this.m_Variable.Set(value);
        public override void Set(GameObject value, GameObject gameObject) => this.m_Variable.Set(value);

        public override GameObject Get(Args args) => this.m_Variable.Get() as GameObject;
        public override GameObject Get(GameObject gameObject) => this.m_Variable.Get() as GameObject;
        
        public static PropertySetGameObject Create => new PropertySetGameObject(
            new SetGameObjectLocalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}