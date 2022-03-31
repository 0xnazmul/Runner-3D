using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Player Bone")]
    [Category("Characters/Player Bone")]
    
    [Description("The bone of a game Object that represents the Player")]
    [Image(typeof(IconBoneSolid), ColorTheme.Type.Green)]

    [Serializable]
    public class GetGameObjectPlayerBone : PropertyTypeGetGameObject
    {
        [SerializeField] private Bone m_Bone = new Bone(HumanBodyBones.RightHand);
        
        public override GameObject Get(Args args)
        {
            GameObject player = ShortcutPlayer.Instance != null 
                ? ShortcutPlayer.Instance.gameObject
                : null;

            if (player == null) return null;
            
            Character character = player.Get<Character>();
            if (character == null) return null;

            return character.Animim?.Animator != null 
                ? this.m_Bone.Get(character.Animim.Animator) 
                : null;
        }

        public override string String => $"Player/{this.m_Bone}";
    }
}