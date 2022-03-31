using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the Game Object value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetGameObjectGlobalName : PropertyTypeSetGameObject
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueGameObject.TYPE_ID);

        public override void Set(GameObject value, Args args) => this.m_Variable.Set(value);
        public override void Set(GameObject value, GameObject gameObject) => this.m_Variable.Set(value);

        public override GameObject Get(Args args) => this.m_Variable.Get() as GameObject;
        public override GameObject Get(GameObject gameObject) => this.m_Variable.Get() as GameObject;
        
        public static PropertySetGameObject Create => new PropertySetGameObject(
            new SetGameObjectGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}