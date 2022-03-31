using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using UnityEditor;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(EntryAnimationClip))]
    public class EntryAnimationClipDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Entry";
    }
}