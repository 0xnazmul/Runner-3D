using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using UnityEditor;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(AnimimBreathing))]
    public class AnimimBreathingDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Breathing";
    }
}