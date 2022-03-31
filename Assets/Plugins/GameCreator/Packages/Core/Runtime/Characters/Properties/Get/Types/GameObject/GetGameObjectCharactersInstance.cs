using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Character")]
    [Category("Characters/Character")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("Reference to a Character game object")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectCharactersInstance : PropertyTypeGetGameObject
    {
        [SerializeField] protected Character m_Character;

        public override GameObject Get(Args args)
        {
            return this.m_Character != null ? this.m_Character.gameObject : null;
        }

        public override GameObject Get(GameObject gameObject)
        {
            return this.m_Character != null ? this.m_Character.gameObject : null;
        }

        public static PropertyGetGameObject Create => new PropertyGetGameObject(
            new GetGameObjectCharactersInstance()
        );

        public override string String => this.m_Character != null
            ? this.m_Character.gameObject.name
            : "(none)";
    }
}