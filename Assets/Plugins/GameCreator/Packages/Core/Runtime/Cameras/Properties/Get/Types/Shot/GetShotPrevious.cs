using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Previous Shot")]
    [Category("Previous Shot")]
    
    [Image(typeof(IconCameraShot), ColorTheme.Type.Yellow, typeof(OverlayArrowLeft))]
    [Description("Reference to the previous Camera Shot used by a Camera")]

    [Serializable]
    public class GetShotPrevious : PropertyTypeGetShot
    {
        [SerializeField] 
        protected PropertyGetCamera m_Camera = GetCameraMain.Create;

        public override ShotCamera Get(Args args)
        {
            TCamera camera = this.m_Camera.Get(args);
            return camera != null ? camera.Transition.PreviousShotCamera : null;
        }

        public static PropertyGetShot Create => new PropertyGetShot(
            new GetShotPrevious()
        );

        public override string String => $"{this.m_Camera} Previous Shot";
    }
}