using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Game Object")]
    [Category("Game Objects/Game Object")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    [Description("Returns the position of the Game Object")]

    [Serializable]
    public class GetPositionGameObject : PropertyTypeGetPosition
    {
        [SerializeField] 
        private PropertyGetGameObject m_GameObject = GetGameObjectInstance.Create();

        public GetPositionGameObject()
        { }
        
        public GetPositionGameObject(GameObject gameObject)
        {
            this.m_GameObject = GetGameObjectInstance.Create(gameObject);
        }
        
        public override Vector3 Get(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            return gameObject != null
                ? gameObject.transform.position
                : default;
        }

        public static PropertyGetPosition Create => new PropertyGetPosition(
            new GetPositionGameObject()
        );

        public override string String => $"{this.m_GameObject}";
    }
}