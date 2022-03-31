using System;
using GameCreator.Runtime.Cameras;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Camera Inverse")]
    [Category("Cameras/Camera Inverse")]
    
    [Image(typeof(IconCamera), ColorTheme.Type.Green, typeof(OverlayArrowLeft))]
    [Description("Inverse rotation of the selected Camera in world space")]

    [Serializable] [HideLabelsInEditor]
    public class GetRotationCameraInverse : PropertyTypeGetRotation
    {
        [SerializeField] private PropertyGetCamera m_Camera = GetCameraMain.Create;
        
        public override Quaternion Get(Args args)
        {
            TCamera camera = this.m_Camera.Get(args);
            return camera != null
                ? Quaternion.LookRotation(
                    camera.transform.TransformDirection(Vector3.back),
                    camera.transform.up
                ) : default;
        }

        public static PropertyGetRotation Create => new PropertyGetRotation(
            new GetRotationCameraInverse()
        );

        public override string String => $"Inverse {this.m_Camera}";
    }
}