using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Cameras
{
    [Title("Main Camera Child Path")]
    [Category("Cameras/Main Camera Child Path")]
    
    [Description("Reference to a child of the Main Camera's game object identified by its name")]
    [Image(typeof(IconCamera), ColorTheme.Type.Green, typeof(OverlayArrowDown))]

    [Serializable]
    public class GetGameObjectCamerasMainPath : PropertyTypeGetGameObject
    {
        [SerializeField] 
        private PropertyGetString m_Path = new PropertyGetString();

        public override GameObject Get(Args args) => this.GetObject(args);

        private GameObject GetObject(Args args)
        {
            if (ShortcutMainCamera.Instance == null) return null;
            Transform transform = ShortcutMainCamera.Instance.transform;
            
            Transform child = transform.Find(this.m_Path.Get(args));
            return child != null ? child.gameObject : null;
        }

        public static PropertyGetGameObject Create => new PropertyGetGameObject(
            new GetGameObjectCamerasMainPath()
        );

        public override string String => $"Main Camera/{this.m_Path}";
    }
}