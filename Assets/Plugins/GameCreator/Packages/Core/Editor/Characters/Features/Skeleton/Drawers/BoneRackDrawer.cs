using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(BoneRack))]
    public class BoneRackDrawer : PropertyDrawer
    {
        private const string USS_PATH = EditorPaths.CHARACTERS + "StyleSheets/BoneRack";
        private const string NAME_BUTTON = "GC-Characters-BoneRack-Button";
        
        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            StyleSheet[] styleSheets = StyleSheetUtils.Load(USS_PATH);
            foreach (StyleSheet styleSheet in styleSheets)
            {
                root.styleSheets.Add(styleSheet);
            }
            
            SerializedProperty display = property.FindPropertyRelative("m_Display");
            SerializedProperty skeleton = property.FindPropertyRelative("m_Skeleton");

            PropertyField fieldDisplay = new PropertyField(display);
            PropertyField fieldSkeleton = new PropertyField(skeleton);

            root.Add(fieldDisplay);
            root.Add(fieldSkeleton);

            return root;
        }
    }
}