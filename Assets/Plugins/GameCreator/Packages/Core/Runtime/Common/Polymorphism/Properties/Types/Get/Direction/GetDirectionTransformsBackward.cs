using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Backward")]
    [Category("Transforms/Backward")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Green, typeof(OverlayArrowDown))]
    [Description("The Transform's backward vector in world space")]

    [Keywords("Game Object")]
    [Serializable]
    public class GetDirectionTransformsBackward : PropertyTypeGetDirection
    {
        [SerializeField]
        protected PropertyGetGameObject m_Transform = GetGameObjectPlayer.Create();

        public GetDirectionTransformsBackward()
        { }
        
        public GetDirectionTransformsBackward(Transform transform)
        {
            this.m_Transform = GetGameObjectTransform.Create(transform);
        }
        
        public override Vector3 Get(Args args)
        {
            GameObject gameObject = this.m_Transform.Get(args);
            return gameObject != null ? -gameObject.transform.forward : default;
        }

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionTransformsBackward()
        );

        public override string String => $"{this.m_Transform} Backward";
    }
}