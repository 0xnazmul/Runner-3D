using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]
    [Description("Returns the Sprite value of a Global List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetSpriteGlobalList : PropertyTypeGetSprite
    {
        [SerializeField]
        protected FieldGetGlobalList m_Variable = new FieldGetGlobalList(ValueSprite.TYPE_ID);

        public override Sprite Get(Args args) => this.m_Variable.Get<Sprite>();
        public override Sprite Get(GameObject gameObject) => this.m_Variable.Get<Sprite>();

        public override string String => this.m_Variable.ToString();
    }
}