using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Player Last Footstep")]
    [Category("Characters/Player Last Footstep")]
    
    [Description("Game Object bone that represents the Player's last foot step")]
    [Image(typeof(IconFootprint), ColorTheme.Type.Green)]

    [Serializable]
    public class GetGameObjectPlayerLastFootstep : PropertyTypeGetGameObject
    {
        public override GameObject Get(Args args)
        {
            Character player = ShortcutPlayer.Instance != null 
                ? ShortcutPlayer.Instance.Get<Character>()
                : null;

            return player != null ? player.Footsteps.LastFootstep : null;
        }

        public static PropertyGetGameObject Create()
        {
            GetGameObjectPlayerLastFootstep instance = new GetGameObjectPlayerLastFootstep();
            return new PropertyGetGameObject(instance);
        }

        public override string String => "Player Footstep";
    }
}