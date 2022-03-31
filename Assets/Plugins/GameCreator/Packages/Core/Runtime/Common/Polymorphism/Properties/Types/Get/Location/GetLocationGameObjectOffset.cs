using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Game Object with Offset")]
    [Category("Game Objects/Game Object with Offset")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayArrowRight))]
    [Description("The position and rotation of a Game Object plus an offset in local space")]

    [Serializable]
    public class GetLocationGameObjectOffset : PropertyTypeGetLocation
    {
        [SerializeField]
        private PropertyGetGameObject m_GameObject = GetGameObjectInstance.Create();
        
        [SerializeField]
        private Vector3 m_LocalOffset = Vector3.forward;
        
        public override Location Get(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            return gameObject != null
                ? new Location(gameObject.transform, this.m_LocalOffset)
                : new Location();
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationGameObjectOffset()
        );

        public override string String => $"{this.m_GameObject}";
    }
}