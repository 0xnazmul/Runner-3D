using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Right")]
    [Category("Transforms/Right")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Green, typeof(OverlayArrowRight))]
    [Description("The Transform's right vector in world space")]

    [Keywords("Game Object")]
    [Serializable]
    public class GetDirectionTransformsRight : PropertyTypeGetDirection
    {
        [SerializeField]
        protected PropertyGetGameObject m_Transform = GetGameObjectPlayer.Create();

        public GetDirectionTransformsRight()
        { }
        
        public GetDirectionTransformsRight(Transform transform)
        {
            this.m_Transform = GetGameObjectTransform.Create(transform);
        }
        
        public override Vector3 Get(Args args)
        {
            GameObject gameObject = this.m_Transform.Get(args);
            return gameObject != null ? gameObject.transform.right : default;
        }

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionTransformsRight()
        );

        public override string String => $"{this.m_Transform} Right";
    }
}