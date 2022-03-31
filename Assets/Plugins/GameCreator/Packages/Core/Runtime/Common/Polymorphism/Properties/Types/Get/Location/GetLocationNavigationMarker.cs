using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Marker")]
    [Category("Navigation/Marker")]
    
    [Image(typeof(IconMarker), ColorTheme.Type.Yellow)]
    [Description("The position and rotation of a Marker component")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationNavigationMarker : PropertyTypeGetLocation
    {
        [SerializeField] private Marker m_Marker;

        public override Location Get(Args args)
        {
            return new Location(this.m_Marker, Vector3.zero);
        }

        public override Location Get(GameObject gameObject)
        {
            return new Location(this.m_Marker, Vector3.zero);
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationNavigationMarker()
        );

        public override string String => this.m_Marker != null 
            ? this.m_Marker.gameObject.name 
            : "(none)";
    }
}