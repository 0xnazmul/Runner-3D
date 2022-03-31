using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Material Color")]
    [Category("Materials/Material Color")]
    
    [Image(typeof(IconSphereSolid), ColorTheme.Type.Yellow)]
    [Description("Returns the material's color")]

    [Serializable] [HideLabelsInEditor]
    public class GetColorMaterialsMaterial : PropertyTypeGetColor
    {
        [SerializeField] protected Material m_Material;

        public override Color Get(Args args) => this.m_Material != null 
            ? this.m_Material.color 
            : default;

        public GetColorMaterialsMaterial() : base()
        { }

        public static PropertyGetColor Create(GameObject gameObject) => new PropertyGetColor(
            new GetColorMaterialsMaterial()
        );

        public override string String => string.Format(
            "{0} Color",
            this.m_Material != null ? this.m_Material.name : "(none)"
        );
    }
}