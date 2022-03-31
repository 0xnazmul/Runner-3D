using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("None")]
    [Category("None")]
    
    [Image(typeof(IconNull), ColorTheme.Type.Yellow)]
    [Description("No offset. Returns a Vector3 with zeros in all components")]

    [Serializable]
    public class GetOffsetNone : PropertyTypeGetOffset
    {
        public GetOffsetNone()
        { }

        public override Vector3 Get(Args args) => Vector3.zero;
        public override Vector3 Get(GameObject transform) => Vector3.zero;

        public static PropertyGetOffset Create => new PropertyGetOffset(
            new GetOffsetNone()
        );

        public override string String => "(none)";
    }
}