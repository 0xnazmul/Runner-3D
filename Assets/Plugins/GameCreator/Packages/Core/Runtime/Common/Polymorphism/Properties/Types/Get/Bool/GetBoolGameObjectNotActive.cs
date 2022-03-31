using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Not Active")]
    [Category("Game Objects/Not Active")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Red)]
    [Description("Returns true if the game object exists and is not active")]
    
    [Keywords("Game Object", "Asset", "Enabled", "Inactive", "Disabled")]
    [Serializable]
    
    public class GetBoolGameObjectNotActive : PropertyTypeGetBool
    {
        [SerializeField]
        protected PropertyGetGameObject m_GameObject = GetGameObjectInstance.Create();

        public override bool Get(Args args)
        {
            GameObject gameObject = this.m_GameObject.Get(args);
            return gameObject != null && !gameObject.activeInHierarchy;
        }

        public GetBoolGameObjectNotActive() : base()
        { }

        public GetBoolGameObjectNotActive(GameObject gameObject) : this()
        {
            this.m_GameObject = GetGameObjectInstance.Create(gameObject);
        }

        public static PropertyGetBool Create(GameObject gameObject) => new PropertyGetBool(
            new GetBoolGameObjectNotActive(gameObject)
        );

        public override string String => $"{this.m_GameObject} not Active";
    }
}