using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Serializable]
    public class PropertyGetCamera : TPropertyGet<PropertyTypeGetCamera, TCamera>
    {
        public PropertyGetCamera() : base(new GetCameraMain())
        { }

        public PropertyGetCamera(PropertyTypeGetCamera defaultType) : base(defaultType)
        { }
    }
}