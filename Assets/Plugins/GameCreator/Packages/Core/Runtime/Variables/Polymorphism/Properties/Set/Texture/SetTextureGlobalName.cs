using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Global Name Variable")]
    [Category("Variables/Global Name Variable")]
    
    [Description("Sets the Texture value of a Global Name Variable")]
    [Image(typeof(IconNameVariable), ColorTheme.Type.Purple, typeof(OverlayDot))]

    [Serializable] [HideLabelsInEditor]
    public class SetTextureGlobalName : PropertyTypeSetTexture
    {
        [SerializeField]
        protected FieldSetGlobalName m_Variable = new FieldSetGlobalName(ValueTexture.TYPE_ID);

        public override void Set(Texture value, Args args) => this.m_Variable.Set(value);
        public override void Set(Texture value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Texture Get(Args args) => this.m_Variable.Get() as Texture;
        public override Texture Get(GameObject gameObject) => this.m_Variable.Get() as Texture;
        
        public static PropertySetTexture Create => new PropertySetTexture(
            new SetTextureGlobalName()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}