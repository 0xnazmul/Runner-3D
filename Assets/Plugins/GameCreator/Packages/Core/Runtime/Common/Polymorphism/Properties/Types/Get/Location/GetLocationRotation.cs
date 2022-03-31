using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Rotation")]
    [Category("Rotation")]
    
    [Image(typeof(IconRotation), ColorTheme.Type.Yellow)]
    [Description("An euler rotation in world space")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationRotation : PropertyTypeGetLocation
    {
        [SerializeField] private Vector3 m_Rotation = Vector3.zero;

        public override Location Get(Args args) => new Location(Quaternion.Euler(this.m_Rotation));
        public override Location Get(GameObject gameObject) => new Location(Quaternion.Euler(this.m_Rotation));

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationRotation()
        );

        public override string String => $"Euler{this.m_Rotation.ToString()}";
    }
}