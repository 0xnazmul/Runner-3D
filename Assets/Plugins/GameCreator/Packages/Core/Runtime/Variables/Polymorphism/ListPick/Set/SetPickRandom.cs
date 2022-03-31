using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Pick Random")]
    [Category("Pick Random")]
    
    [Description("Replaces a random element between the first and last position of the list")]
    [Image(typeof(IconDice), ColorTheme.Type.Red)]

    [Serializable]
    public class SetPickRandom : IListSetPick
    {
        public int GetIndex(ListVariableRuntime list, int count)
        {
            return UnityEngine.Random.Range(0, count);
        }

        public int GetIndex(int count)
        {
            return UnityEngine.Random.Range(0, count);
        }

        public override string ToString() => "Random";
    }
}