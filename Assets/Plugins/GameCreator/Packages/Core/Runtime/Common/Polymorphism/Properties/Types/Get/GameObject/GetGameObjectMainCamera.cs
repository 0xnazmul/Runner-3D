using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Main Camera")]
    [Category("Cameras/Main Camera")]
    
    [Description("Game Object that represents the Main Camera")]
    [Image(typeof(IconCamera), ColorTheme.Type.Green)]

    [Serializable]
    public class GetGameObjectMainCamera : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args)
        {
            return ShortcutMainCamera.Instance != null 
                ? ShortcutMainCamera.Instance.gameObject
                : null;
        }

        public override GameObject Get(GameObject gameObject)
        {
            return ShortcutMainCamera.Instance != null 
                ? ShortcutMainCamera.Instance.gameObject
                : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectMainCamera instance = new GetGameObjectMainCamera();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Main Camera";
    }
}