using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Position")]
    [Category("Position")]

    [Image(typeof(IconVector3), ColorTheme.Type.Yellow)]
    [Description("A point in space representing a position in the scene")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationPosition : PropertyTypeGetLocation
    {
        [SerializeField] private Vector3 m_Position = Vector3.zero;

        public override Location Get(Args args) => new Location(this.m_Position);
        public override Location Get(GameObject gameObject) => new Location(this.m_Position);

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationPosition()
        );

        public override string String => this.m_Position.ToString();
    }
}