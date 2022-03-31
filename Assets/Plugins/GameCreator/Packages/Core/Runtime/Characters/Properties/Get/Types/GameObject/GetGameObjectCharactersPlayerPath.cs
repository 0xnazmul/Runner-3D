using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Player Child Path")]
    [Category("Characters/Player Child Path")]
    
    [Description("Reference to a child of the Player's game object identified by its name")]
    [Image(typeof(IconPlayer), ColorTheme.Type.Green, typeof(OverlayArrowDown))]

    [Serializable]
    public class GetGameObjectCharactersPlayerPath : PropertyTypeGetGameObject
    {
        [SerializeField] 
        private PropertyGetString m_Path = new PropertyGetString();

        public override GameObject Get(Args args) => this.GetObject(args);

        private GameObject GetObject(Args args)
        {
            if (ShortcutPlayer.Instance == null) return null;
            Transform transform = ShortcutPlayer.Transform;
            
            Transform child = transform.Find(this.m_Path.Get(args));
            return child != null ? child.gameObject : null;
        }

        public static PropertyGetGameObject Create => new PropertyGetGameObject(
            new GetGameObjectCharactersPlayerPath()
        );

        public override string String => $"Player/{this.m_Path}";
    }
}