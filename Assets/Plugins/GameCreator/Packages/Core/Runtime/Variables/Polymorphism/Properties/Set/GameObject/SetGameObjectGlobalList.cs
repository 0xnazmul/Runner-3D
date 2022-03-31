using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the Game Object value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetGameObjectGlobalList : PropertyTypeSetGameObject
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueGameObject.TYPE_ID);

        public override void Set(GameObject value, Args args) => this.m_Variable.Set(value);
        public override void Set(GameObject value, GameObject gameObject) => this.m_Variable.Set(value);

        public override GameObject Get(Args args) => this.m_Variable.Get() as GameObject;
        public override GameObject Get(GameObject gameObject) => this.m_Variable.Get() as GameObject;
        
        public static PropertySetGameObject Create => new PropertySetGameObject(
            new SetGameObjectGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}