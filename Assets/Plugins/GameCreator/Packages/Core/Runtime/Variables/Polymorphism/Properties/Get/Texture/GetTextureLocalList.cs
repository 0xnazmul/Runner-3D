using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Local List Variable")]
    [Category("Variables/Local List Variable")]
    
    [Image(typeof(IconListVariable), ColorTheme.Type.Teal)]
    [Description("Returns the Texture value of a Local List Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetTextureLocalList : PropertyTypeGetTexture
    {
        [SerializeField]
        protected FieldGetLocalList m_Variable = new FieldGetLocalList(ValueTexture.TYPE_ID);

        public override Texture Get(Args args) => this.m_Variable.Get<Texture>();
        public override Texture Get(GameObject gameObject) => this.m_Variable.Get<Texture>();

        public override string String => this.m_Variable.ToString();
    }
}