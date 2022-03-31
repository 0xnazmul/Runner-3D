using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Cameras
{
    [Title("Main Camera")]
    [Category("Main Camera")]
    
    [Image(typeof(IconCamera), ColorTheme.Type.Green)]
    [Description("Camera that represents the Main Camera")]

    [Serializable]
    public class GetCameraMain : PropertyTypeGetCamera
    {
        public override TCamera Get(Args args)
        {
            return ShortcutMainCamera.Instance != null 
                ? ShortcutMainCamera.Instance.Get<TCamera>()
                : null;
        }

        public override TCamera Get(GameObject gameObject)
        {
            return ShortcutMainCamera.Instance != null 
                ? ShortcutMainCamera.Instance.Get<TCamera>()
                : null;
        }

        public static PropertyGetCamera Create => new PropertyGetCamera(
            new GetCameraMain()
        );

        public override string String => "Main Camera";
    }
}