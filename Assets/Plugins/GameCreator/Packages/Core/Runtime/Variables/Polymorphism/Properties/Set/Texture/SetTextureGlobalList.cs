using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global List Variable")]
    [Category("Variables/Global List Variable")]
    
    [Description("Sets the Texture value of a Global List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetTextureGlobalList : PropertyTypeSetTexture
    {
        [SerializeField]
        protected FieldSetGlobalList m_Variable = new FieldSetGlobalList(ValueTexture.TYPE_ID);

        public override void Set(Texture value, Args args) => this.m_Variable.Set(value);
        public override void Set(Texture value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Texture Get(Args args) => this.m_Variable.Get() as Texture;
        public override Texture Get(GameObject gameObject) => this.m_Variable.Get() as Texture;
        
        public static PropertySetTexture Create => new PropertySetTexture(
            new SetTextureGlobalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}