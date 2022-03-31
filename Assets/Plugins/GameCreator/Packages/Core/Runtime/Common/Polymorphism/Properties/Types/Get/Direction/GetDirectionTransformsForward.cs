using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Forward")]
    [Category("Transforms/Forward")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Green, typeof(OverlayArrowUp))]
    [Description("The Transform's forward vector in world space")]

    [Keywords("Game Object")]
    [Serializable]
    public class GetDirectionTransformsForward : PropertyTypeGetDirection
    {
        [SerializeField]
        protected PropertyGetGameObject m_Transform = GetGameObjectPlayer.Create();

        public GetDirectionTransformsForward()
        { }
        
        public GetDirectionTransformsForward(Transform transform)
        {
            this.m_Transform = GetGameObjectTransform.Create(transform);
        }
        
        public override Vector3 Get(Args args)
        {
            GameObject gameObject = this.m_Transform.Get(args);
            return gameObject != null ? gameObject.transform.forward : default;
        }

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionTransformsForward()
        );

        public override string String => $"{this.m_Transform} Forward";
    }
}