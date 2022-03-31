using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("First Element")]
    [Category("First Element")]
    
    [Description("Replaces the element that appears first on the list")]
    [Image(typeof(IconListFirst), ColorTheme.Type.Yellow)]

    [Serializable]
    public class SetPickFirst : IListSetPick
    {
        public int GetIndex(ListVariableRuntime list, int count) => 0;
        public int GetIndex(int count) => 0;

        public override string ToString() => "First";
    }
}