using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Target")]
    [Category("Target")]

    [Image(typeof(IconTarget), ColorTheme.Type.Yellow)]
    [Description("The position and/or rotation of the targeted game object")]

    [Serializable]
    public class GetLocationTarget : PropertyTypeGetLocation
    {
        [SerializeField] private bool m_Position = true;
        [SerializeField] private bool m_Rotation = false;

        public override Location Get(Args args)
        {
            if (args.Target == null) return new Location();
            return new Location(
                this.m_Position,
                this.m_Rotation,
                this.m_Position ? args.Target.transform.position : Vector3.zero,
                this.m_Rotation ? args.Target.transform.rotation : Quaternion.identity
            );
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationTarget()
        );

        public override string String => "Target";
    }
}