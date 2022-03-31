using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Pick Random")]
    [Category("Pick Random")]
    
    [Description("Selects a random index between the first and last elements of the list")]
    [Image(typeof(IconDice), ColorTheme.Type.Red)]

    [Serializable]
    public class GetPickRandom : IListGetPick
    {
        public int GetIndex(int count)
        {
            return UnityEngine.Random.Range(0, count);
        }

        public override string ToString() => "Random";
    }
}