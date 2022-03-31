using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class PropertyGetOffset : TPropertyGet<PropertyTypeGetOffset, Vector3>
    {
        public PropertyGetOffset() : base(new GetOffsetVector3())
        { }

        public PropertyGetOffset(PropertyTypeGetOffset defaultType) : base(defaultType)
        { }

        public PropertyGetOffset(Vector3 worldSpaceOffset) : base(new GetOffsetVector3(worldSpaceOffset))
        { }
    }
}