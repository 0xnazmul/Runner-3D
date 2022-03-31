using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class PropertyGetDirection : TPropertyGet<PropertyTypeGetDirection, Vector3>
    {
        public PropertyGetDirection() : base(new GetDirectionVector3())
        { }
        
        public PropertyGetDirection(Vector3 direction) : base(new GetDirectionVector3(direction))
        { }

        public PropertyGetDirection(PropertyTypeGetDirection defaultType) : base(defaultType)
        { }
    }
}