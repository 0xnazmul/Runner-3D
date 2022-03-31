using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Common
{
    internal class TypeChapter
    {
        public TypePage First { get; }

        public TypeChapter(Type type)
        {
            Tree<Type> tree = TypeUtils.GetTypesTree(type);
            this.First = new TypePage(tree, false);
        }
    }
}
