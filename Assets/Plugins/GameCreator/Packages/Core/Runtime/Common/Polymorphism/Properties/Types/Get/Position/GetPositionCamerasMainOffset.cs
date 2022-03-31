using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Main Camera with Offset")]
    [Category("Cameras/Main Camera with Offset")]
    
    [Image(typeof(IconCamera), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Description("Returns the position of the Main Camera object plus an offset in local space")]

    [Serializable] [HideLabelsInEditor]
    public class GetPositionCamerasMainOffset : PropertyTypeGetPosition
    {
        [SerializeField] private Vector3 m_LocalOffset = Vector3.forward;
        
        public override Vector3 Get(Args args)
        {
            Transform transform = ShortcutMainCamera.Transform;
            return transform != null 
                ? transform.position + transform.TransformDirection(this.m_LocalOffset) 
                : default;
        }
        
        public override Vector3 Get(GameObject gameObject)
        {
            Transform transform = ShortcutMainCamera.Transform;
            return transform != null
                ? transform.position + transform.TransformDirection(this.m_LocalOffset) 
                : default;
        }

        public static PropertyGetPosition Create => new PropertyGetPosition(
            new GetPositionCamerasMainOffset()
        );

        public override string String => "Main Camera";
    }
}