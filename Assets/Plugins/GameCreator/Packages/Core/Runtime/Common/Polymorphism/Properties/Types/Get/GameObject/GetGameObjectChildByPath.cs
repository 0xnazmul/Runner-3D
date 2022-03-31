using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Game Object Child Path")]
    [Category("Transforms/Game Object Child Path")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Blue, typeof(OverlayArrowDown))]
    [Description("The child of a game object found in its hierarchy identified by its name")]

    [Serializable]
    public class GetGameObjectChildByPath : PropertyTypeGetGameObject
    {
        [SerializeField] 
        private PropertyGetGameObject m_Transform = GetGameObjectPlayer.Create();
        
        [SerializeField] 
        private PropertyGetString m_Path = GetStringString.Create;

        public override GameObject Get(Args args)
        {
            GameObject gameObject = this.m_Transform.Get(args);
            if (gameObject == null) return null;

            Transform child = gameObject.transform.Find(this.m_Path.Get(args));
            return child != null ? child.gameObject : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectChildByPath instance = new GetGameObjectChildByPath();
            return new PropertyGetGameObject(instance);
        }

        public override string String => $"{this.m_Transform}/{this.m_Path}";
    }
}