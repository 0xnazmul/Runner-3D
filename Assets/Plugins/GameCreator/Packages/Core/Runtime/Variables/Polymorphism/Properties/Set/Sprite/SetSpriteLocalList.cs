using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the Sprite value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetSpriteLocalList : PropertyTypeSetSprite
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueSprite.TYPE_ID);

        public override void Set(Sprite value, Args args) => this.m_Variable.Set(value);
        public override void Set(Sprite value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Sprite Get(Args args) => this.m_Variable.Get() as Sprite;
        public override Sprite Get(GameObject gameObject) => this.m_Variable.Get() as Sprite;
        
        public static PropertySetSprite Create => new PropertySetSprite(
            new SetSpriteLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}