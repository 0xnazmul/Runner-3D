using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Left")]
    [Category("Transforms/Left")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Green, typeof(OverlayArrowLeft))]
    [Description("The Transform's left vector in world space")]

    [Keywords("Game Object")]
    [Serializable]
    public class GetDirectionTransformsLeft : PropertyTypeGetDirection
    {
        [SerializeField] 
        protected PropertyGetGameObject m_Transform = GetGameObjectPlayer.Create();

        public GetDirectionTransformsLeft()
        { }
        
        public GetDirectionTransformsLeft(Transform transform)
        {
            this.m_Transform = GetGameObjectTransform.Create(transform);
        }
        
        public override Vector3 Get(Args args)
        {
            GameObject gameObject = this.m_Transform.Get(args);
            return gameObject != null ? -gameObject.transform.right : default;
        }

        public static PropertyGetDirection Create => new PropertyGetDirection(
            new GetDirectionTransformsLeft()
        );

        public override string String => $"{this.m_Transform} Left";
    }
}