using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Character")]
    [Category("Characters/Character")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("Returns the position of the Character")]

    [Serializable]
    public class GetPositionCharacter : PropertyTypeGetPosition
    {
        [SerializeField]
        protected Character m_Character;
        
        public override Vector3 Get(Args args)
        {
            return this.m_Character != null 
                ? this.m_Character.transform.position 
                : default;
        }

        public static PropertyGetPosition Create => new PropertyGetPosition(
            new GetPositionCharacter()
        );

        public override string String => this.m_Character != null
            ? this.m_Character.gameObject.name
            : "(none)";
    }
}