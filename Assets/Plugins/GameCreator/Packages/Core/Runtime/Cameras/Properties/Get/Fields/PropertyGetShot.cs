using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class PropertyGetShot : TPropertyGet<PropertyTypeGetShot, ShotCamera>
    {
        public PropertyGetShot() : base(new GetShotInstance())
        { }

        public PropertyGetShot(PropertyTypeGetShot defaultType) : base(defaultType)
        { }
    }
}