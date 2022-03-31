using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Game Object with Offset")]
    [Category("Game Objects/Game Object with Offset")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    [Description("Returns the position of the Game Object plus an offset in local space")]

    [Serializable]
    public class GetPositionGameObjectOffset : PropertyTypeGetPosition
    {
        [SerializeField] 
        private PropertyGetGameObject m_GameObject = GetGameObjectInstance.Create();
        
        [SerializeField]
        private Vector3 m_LocalOffset = Vector3.forward;

        public GetPositionGameObjectOffset()
        { }
        
        public GetPositionGameObjectOffset(GameObject gameObject)
        {
            this.m_GameObject = GetGameObjectInstance.Create(gameObject);
        }
        
        public override Vector3 Get(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            if (gameObject == null) return default;

            Vector3 localOffset = gameObject.transform.TransformDirection(this.m_LocalOffset);
            return gameObject.transform.position + localOffset;
        }

        public static PropertyGetPosition Create => new PropertyGetPosition(
            new GetPositionGameObjectOffset()
        );

        public override string String => $"{this.m_GameObject}";
    }
}