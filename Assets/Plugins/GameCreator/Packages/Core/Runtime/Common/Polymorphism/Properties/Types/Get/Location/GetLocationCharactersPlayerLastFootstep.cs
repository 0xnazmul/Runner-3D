using System;
using GameCreator.Runtime.Characters;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Player Last Footstep")]
    [Category("Characters/Player Last Footstep")]
    
    [Image(typeof(IconFootprint), ColorTheme.Type.Green)]
    [Description("The position and rotation of the Player's last step bone")]

    [Serializable]
    public class GetLocationCharactersPlayerLastFootstep : PropertyTypeGetLocation
    {
        public override Location Get(Args args)
        {
            Character player = ShortcutPlayer.Instance != null 
                ? ShortcutPlayer.Instance.Get<Character>()
                : null;

            return player != null 
                ? new Location(player.Footsteps.LastFootstep, Vector3.zero) 
                : new Location();
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationCharactersPlayerLastFootstep()
        );

        public override string String => "Player Last Footstep";
    }
}