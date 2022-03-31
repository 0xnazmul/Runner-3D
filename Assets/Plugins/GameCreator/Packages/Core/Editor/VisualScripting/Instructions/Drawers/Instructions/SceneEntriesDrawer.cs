using GameCreator.Editor.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEditor;

namespace GameCreator.Editor.VisualScripting
{
    [CustomPropertyDrawer(typeof(InstructionCommonSceneLoad.SceneEntries))]
    public class SceneEntriesDrawer : TSectionDrawer
    {
        protected override string Name(SerializedProperty property)
        {
            return "Scene Entries";
        }
    }
}
