using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Player with Offset")]
    [Category("Characters/Player with Offset")]
    
    [Image(typeof(IconPlayer), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Description("Returns the position of the Player character plus an offset in local space")]

    [Serializable] [HideLabelsInEditor]
    public class GetPositionCharactersPlayerOffset : PropertyTypeGetPosition
    {
        [SerializeField] private Vector3 m_LocalOffset = Vector3.forward;
        
        public override Vector3 Get(Args args) => this.GetPosition();
        public override Vector3 Get(GameObject gameObject) => this.GetPosition();

        private Vector3 GetPosition()
        {
            Transform transform = ShortcutPlayer.Transform;
            return transform != null 
                ? transform.position + transform.TransformDirection(this.m_LocalOffset)  
                : default;
        }

        public static PropertyGetPosition Create => new PropertyGetPosition(
            new GetPositionCharactersPlayerOffset()
        );

        public override string String => "Player";
    }
}