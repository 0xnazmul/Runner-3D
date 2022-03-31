using System;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Title("Character Bone")]
    [Category("Characters/Character Bone")]
    
    [Image(typeof(IconBoneSolid), ColorTheme.Type.Yellow)]
    [Description("The bone references on a Character game object")]

    [Serializable] [HideLabelsInEditor]
    public class GetGameObjectCharactersBone : PropertyTypeGetGameObject
    {
        [SerializeField] protected Character m_Character;
        [SerializeField] private Bone m_Bone = new Bone(HumanBodyBones.RightHand);

        public override GameObject Get(Args args)
        {
            if (this.m_Character == null) return null;
            return this.m_Character.Animim?.Animator != null 
                ? this.m_Bone.Get(this.m_Character.Animim?.Animator) 
                : null;
        }

        public override string String => string.Format(
            "{0}/{1}",
            this.m_Character != null ? this.m_Character.gameObject.name : "(none)",
            this.m_Bone
        );
    }
}