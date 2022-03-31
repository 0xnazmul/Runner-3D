using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Insert First Element")]
    [Category("Insert First Element")]
    
    [Description("Inserts a new element as the first one on the list")]
    [Image(typeof(IconListFirst), ColorTheme.Type.Blue)]

    [Serializable]
    public class SetPickInsertFirst : IListSetPick
    {
        public int GetIndex(ListVariableRuntime list, int count)
        {
            list.Insert(0, default);
            return 0;
        }
        
        public int GetIndex(int count) => -1;

        public override string ToString() => "Insert First";
    }
}