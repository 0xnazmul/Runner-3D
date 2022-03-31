using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Description("Sets the Texture value of a Local List Variable")]
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]

    [Serializable] [HideLabelsInEditor]
    public class SetTextureLocalList : PropertyTypeSetTexture
    {
        [SerializeField]
        protected FieldSetLocalList m_Variable = new FieldSetLocalList(ValueTexture.TYPE_ID);

        public override void Set(Texture value, Args args) => this.m_Variable.Set(value);
        public override void Set(Texture value, GameObject gameObject) => this.m_Variable.Set(value);

        public override Texture Get(Args args) => this.m_Variable.Get() as Texture;
        public override Texture Get(GameObject gameObject) => this.m_Variable.Get() as Texture;
        
        public static PropertySetTexture Create => new PropertySetTexture(
            new SetTextureLocalList()
        );
        
        public override string String => this.m_Variable.ToString();
    }
}