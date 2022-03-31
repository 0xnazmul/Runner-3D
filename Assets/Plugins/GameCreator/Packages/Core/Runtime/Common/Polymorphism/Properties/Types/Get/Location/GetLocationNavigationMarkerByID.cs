using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Marker by ID")]
    [Category("Navigation/Marker by ID")]
    
    [Image(typeof(IconID), ColorTheme.Type.TextNormal)]
    [Description("The position and rotation of a Marker in the scene identified by its ID")]

    [Serializable] [HideLabelsInEditor]
    public class GetLocationNavigationMarkerByID : PropertyTypeGetLocation
    {
        [SerializeField]
        private PropertyGetString m_ID = new PropertyGetString("my-marker-id");

        public override Location Get(Args args) => this.GetLocation(args);

        private Location GetLocation(Args args)
        {
            string id = this.m_ID.Get(args);
            return new Location(Marker.GetMarkerByID(id), Vector3.zero);
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationNavigationMarkerByID()
        );

        public override string String => $"Marker ID:{this.m_ID}";
    }
}