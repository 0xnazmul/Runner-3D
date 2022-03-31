using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Not Exists")]
    [Category("Game Objects/Not Exists")]
    
    [Image(typeof(IconCubeOutline), ColorTheme.Type.Red)]
    [Description("Returns true if the game object does not exist")]
    
    [Keywords("Game Object", "Asset")]
    [Serializable]
    public class GetBoolGameObjectNotExists : PropertyTypeGetBool
    {
        [SerializeField] protected PropertyGetGameObject m_GameObject = GetGameObjectInstance.Create();

        public override bool Get(Args args) => this.m_GameObject.Get(args) == null;

        public GetBoolGameObjectNotExists() : base()
        { }

        public GetBoolGameObjectNotExists(GameObject gameObject) : this()
        {
            this.m_GameObject = GetGameObjectInstance.Create(gameObject);
        }

        public static PropertyGetBool Create(GameObject gameObject) => new PropertyGetBool(
            new GetBoolGameObjectNotExists(gameObject)
        );

        public override string String => $"{this.m_GameObject} not Exists";
    }
}