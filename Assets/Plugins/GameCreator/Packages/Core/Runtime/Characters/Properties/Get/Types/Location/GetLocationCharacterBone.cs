using System;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Title("Character Bone")]
    [Category("Characters/Character Bone")]
    
    [Image(typeof(IconBoneSolid), ColorTheme.Type.Yellow)]
    [Description("The position and rotation of a Character bone")]

    [Serializable]
    public class GetLocationCharacterBone : PropertyTypeGetLocation
    {
        [SerializeField] protected PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
        [SerializeField] protected Bone m_Bone = new Bone(HumanBodyBones.RightHand);

        public override Location Get(Args args)
        {
            Character character = this.m_Character.Get<Character>(args);
            if (character == null) return default;

            return new Location(
                this.m_Bone.GetTransform(character.Animim?.Animator), 
                Vector3.zero
            );
        }

        public override string String => $"{this.m_Character}/{this.m_Bone}";
    }
}