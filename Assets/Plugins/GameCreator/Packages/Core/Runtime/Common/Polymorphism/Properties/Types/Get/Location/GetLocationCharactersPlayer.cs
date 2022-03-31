using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [Title("Player")]
    [Category("Characters/Player")]
    
    [Image(typeof(IconPlayer), ColorTheme.Type.Green)]
    [Description("The position and rotation of the Player")]

    [Serializable]
    public class GetLocationCharactersPlayer : PropertyTypeGetLocation
    {
        public override Location Get(Args args)
        {
            return new Location(ShortcutPlayer.Transform, Vector3.zero);
        }

        public override Location Get(GameObject gameObject)
        {
            return new Location(ShortcutPlayer.Transform, Vector3.zero);
        }

        public static PropertyGetLocation Create => new PropertyGetLocation(
            new GetLocationCharactersPlayer()
        );

        public override string String => "Player";
    }
}