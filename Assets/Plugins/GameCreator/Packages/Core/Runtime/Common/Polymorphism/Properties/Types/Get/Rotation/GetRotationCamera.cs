using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Camera")]
    [Category("Cameras/Camera")]
    
    [Image(typeof(IconCamera), ColorTheme.Type.Green)]
    [Description("Rotation of the selected Camera in world space")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationCamera : PropertyTypeGetRotation
    {
        [SerializeField] private PropertyGetCamera m_Camera = GetCameraMain.Create;
        
        public override Quaternion Get(Args args)
        {
            TCamera camera = this.m_Camera.Get(args);
            return camera != null ? camera.transform.rotation : default;
        }

        public static PropertyGetRotation Create => new PropertyGetRotation(
            new GetRotationCamera()
        );

        public override string String => this.m_Camera.ToString();
    }
}