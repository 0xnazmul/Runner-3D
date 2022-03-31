using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Game Object")]
    [Category("Game Objects/Game Object")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue)]
    [Description("The position and rotation of a Game Object")]

    [Serializable]
    public class GetLocationGameObject : PropertyTypeGetLocation
    {
        [SerializeField]
        private PropertyGetGameObject m_GameObject = GetGameObjectInstance.Create();
        
        public override Location Get(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            return gameObject != null
                ? new Location(gameObject.transform, Vector3.zero)
                : new Location();
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationGameObject()
        );

        public override string String => $"{this.m_GameObject}";
    }
}