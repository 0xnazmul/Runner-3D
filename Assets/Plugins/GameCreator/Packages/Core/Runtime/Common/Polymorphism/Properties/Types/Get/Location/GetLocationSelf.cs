using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Self")]
    [Category("Self")]

    [Image(typeof(IconSelf), ColorTheme.Type.Yellow)]
    [Description("The position and/or rotation of the caller game object")]

    [Serializable]
    public class GetLocationSelf : PropertyTypeGetLocation
    {
        [SerializeField] private bool m_Position = true;
        [SerializeField] private bool m_Rotation = false;

        public override Location Get(Args args)
        {
            if (args.Self == null) return new Location();
            return new Location(
                this.m_Position,
                this.m_Rotation,
                this.m_Position ? args.Self.transform.position : Vector3.zero,
                this.m_Rotation ? args.Self.transform.rotation : Quaternion.identity
            );
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationSelf()
        );

        public override string String => "Self";
    }
}