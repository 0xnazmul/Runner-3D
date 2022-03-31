using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Sphere Surface")]
    [Category("Random/Sphere Surface")]
    
    [Image(typeof(IconDice), ColorTheme.Type.White)]
    [Description("Returns a random position inside a spherical volume")]

    [Serializable] [HideLabelsInEditor]
    public class GetPositionRandomSphereSurface : PropertyTypeGetPosition
    {
        [SerializeField] protected float m_Radius = 1f;

        public override Vector3 Get(Args args)
        {
            return UnityEngine.Random.onUnitSphere * this.m_Radius;
        }

        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionRandomSphereSurface()
        );

        public override string String => "on Sphere";
    }
}