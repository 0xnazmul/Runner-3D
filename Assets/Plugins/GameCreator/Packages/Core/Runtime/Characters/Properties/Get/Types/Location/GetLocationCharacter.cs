using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Character")]
    [Category("Characters/Character")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("The position and rotation of the Character")]

    [Serializable]
    public class GetLocationCharacter : PropertyTypeGetLocation
    {
        [SerializeField]
        protected PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        
        public override Location Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null 
                ? new Location(character.transform, Vector3.zero) 
                : new Location();
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationCharacter()
        );

        public override string String => $"{this.m_Character}";
    }
}