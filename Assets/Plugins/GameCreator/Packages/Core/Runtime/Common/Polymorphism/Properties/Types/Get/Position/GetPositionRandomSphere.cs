using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Sphere Volume")]
    [Category("Random/Sphere Volume")]
    
    [Image(typeof(IconDice), ColorTheme.Type.White)]
    [Description("Returns a random position inside a spherical volume")]

    [Serializable] [HideLabelsInEditor]
    public class GetPositionRandomSphere : PropertyTypeGetPosition
    {
        [SerializeField] protected float m_Radius = 1f;

        public override Vector3 Get(Args args)
        {
            return UnityEngine.Random.insideUnitSphere * this.m_Radius;
        }

        public static PropertyGetPosition Create() => new PropertyGetPosition(
            new GetPositionRandomSphere()
        );

        public override string String => "in Sphere";
    }
}