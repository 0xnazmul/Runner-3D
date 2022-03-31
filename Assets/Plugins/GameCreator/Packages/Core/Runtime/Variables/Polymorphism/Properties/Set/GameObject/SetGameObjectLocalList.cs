using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the Game Object value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetGameObjectLocalList : PropertyTypeSetGameObject
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueGameObject.TYPE_ID);

        public override void Set(GameObject value, Args args) => this.m_Variable.Set(value);
        public override void Set(GameObject value, GameObject gameObject) => this.m_Variable.Set(value);

        public override GameObject Get(Args args) => this.m_Variable.Get() as GameObject;
        public override GameObject Get(GameObject gameObject) => this.m_Variable.Get() as GameObject;
        
        public static PropertySetGameObject Create => new PropertySetGameObject(
            new SetGameObjectLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}