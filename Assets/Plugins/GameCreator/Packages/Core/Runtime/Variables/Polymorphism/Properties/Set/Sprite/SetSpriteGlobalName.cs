using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the Sprite value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetSpriteGlobalName : PropertyTypeSetSprite
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueSprite.TYPE_ID);

        public override void Set(Sprite value, Args args) => this.m_Variable.Set(value);
        public override void Set(Sprite value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Sprite Get(Args args) => this.m_Variable.Get() as Sprite;
        public override Sprite Get(GameObject gameObject) => this.m_Variable.Get() as Sprite;
        
        public static PropertySetSprite Create => new PropertySetSprite(
            new SetSpriteGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}