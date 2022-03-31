using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using UnityEditor;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(AnimimTwitching))]
    public class AnimimTwitchingDrawer : TBoxDrawer
    {
        protected override string Name(SerializedProperty property) => "Twitching";
    }
}