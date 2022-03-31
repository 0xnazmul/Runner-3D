using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Camera")]
    [Category("Camera")]
    
    [Image(typeof(IconCamera), ColorTheme.Type.Blue)]
    [Description("Reference to a Camera game object")]

    [Serializable] [HideLabelsInEditor]
    public class GetCameraInstance : PropertyTypeGetCamera
    {
        [SerializeField] protected TCamera m_Camera;

        public override TCamera Get(Args args) => this.m_Camera;
        public override TCamera Get(GameObject gameObject) => this.m_Camera;

        public static PropertyGetCamera Create => new PropertyGetCamera(
            new GetCameraInstance()
        );

        public override string String => this.m_Camera != null
            ? this.m_Camera.gameObject.name
            : "(none)";
    }
}