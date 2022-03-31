using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Character with Offset")]
    [Category("Characters/Character with Offset")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    [Description("The position and rotation of the Character plus an offset in local space")]

    [Serializable]
    public class GetLocationCharacterLocalOffset : PropertyTypeGetLocation
    {
        [SerializeField]
        protected PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        
        [SerializeField]
        private Vector3 m_LocalOffset = Vector3.forward;
        
        public override Location Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            return character != null 
                ? new Location(character.transform, this.m_LocalOffset) 
                : new Location();
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationCharacterLocalOffset()
        );

        public override string String => $"{this.m_Character}";
    }
}