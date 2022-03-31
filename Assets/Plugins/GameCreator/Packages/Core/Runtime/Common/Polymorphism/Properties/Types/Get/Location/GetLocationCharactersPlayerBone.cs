using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Player Bone")]
    [Category("Characters/Player Bone")]
    
    [Image(typeof(IconBoneSolid), ColorTheme.Type.Green)]
    [Description("The position and rotation of a Player's bone")]

    [Serializable]
    public class GetLocationCharactersPlayerBone : PropertyTypeGetLocation
    {
        [SerializeField] private Bone m_Bone = new Bone(HumanBodyBones.RightHand);
        
        public override Location Get(Args args)
        {
            Character character = ShortcutPlayer.Get<Character>();
            if (character == null) return default;

            Transform bone = this.m_Bone.GetTransform(character.Animim?.Animator);
            return new Location(bone, Vector3.zero);
        }

        public override string String => $"Player/{this.m_Bone}";
    }
}