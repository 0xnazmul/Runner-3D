using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Last Element")]
    [Category("Last Element")]
    
    [Description("Replaces the element that's at the bottom of the list")]
    [Image(typeof(IconListLast), ColorTheme.Type.Yellow)]

    [Serializable]
    public class SetPickLast : IListSetPick
    {
        public int GetIndex(ListVariableRuntime list, int count) => count - 1;
        public int GetIndex(int count) => count - 1;
        
        public override string ToString() => "Last";
    }
}