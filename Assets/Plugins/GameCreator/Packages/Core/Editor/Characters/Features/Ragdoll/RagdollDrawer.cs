using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using UnityEditor;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(Ragdoll))]
    public class RagdollDrawer : TSectionDrawer
    {
        protected override string Name(SerializedProperty property) => "Ragdoll";
    }
}
