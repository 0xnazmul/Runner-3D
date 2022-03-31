using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Variables
{
    [Title("Insert Last Element")]
    [Category("Insert Last Element")]
    
    [Description("Inserts a new element at the end on the list")]
    [Image(typeof(IconListLast), ColorTheme.Type.Blue)]

    [Serializable]
    public class SetPickInsertLast : IListSetPick
    {
        public int GetIndex(ListVariableRuntime list, int count)
        {
            list.Insert(count, default);
            return count;
        }
        
        public int GetIndex(int count) => -1;

        public override string ToString() => "Insert Last";
    }
}