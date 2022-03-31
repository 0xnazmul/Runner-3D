using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    [CustomPropertyDrawer(typeof(PoolField))]
    public class PoolFieldDrawer : PropertyDrawer
    {
        private const string CLASS_INLINE_FIELD = "gc-inline-toggle-field";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            StyleSheet[] sheets = StyleSheetUtils.Load();
            foreach (StyleSheet sheet in sheets) root.styleSheets.Add(sheet);

            SerializedProperty prefab = property.FindPropertyRelative("m_Prefab");
            SerializedProperty usePooling = property.FindPropertyRelative("m_UsePooling");
            SerializedProperty size = property.FindPropertyRelative("m_Size");
            SerializedProperty hasDuration = property.FindPropertyRelative("m_HasDuration");
            SerializedProperty duration = property.FindPropertyRelative("m_Duration");

            Toggle fieldUsePooling = new Toggle
            {
                label = usePooling.displayName,
                bindingPath = usePooling.propertyPath
            };
            
            PropertyField fieldSize = new PropertyField(size, string.Empty);
            
            Toggle fieldHasDuration = new Toggle
            {
                label = hasDuration.displayName,
                bindingPath = hasDuration.propertyPath
            };
            
            PropertyField fieldDuration = new PropertyField(duration, string.Empty);

            VisualElement contentPooling = new VisualElement();
            VisualElement contentDuration = new VisualElement();
            
            contentPooling.AddToClassList(CLASS_INLINE_FIELD);
            contentDuration.AddToClassList(CLASS_INLINE_FIELD);
            
            contentPooling.Add(fieldUsePooling);
            contentPooling.Add(fieldSize);
            contentDuration.Add(fieldHasDuration);
            contentDuration.Add(fieldDuration);
            
            root.Add(new PropertyField(prefab));
            root.Add(contentPooling);
            root.Add(contentDuration);

            fieldUsePooling.RegisterValueChangedCallback(changeEvent =>
            {
                fieldSize.SetEnabled(changeEvent.newValue);
                contentDuration.style.display = changeEvent.newValue
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            });
            
            fieldSize.SetEnabled(usePooling.boolValue);
            contentDuration.style.display = usePooling.boolValue
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            fieldHasDuration.RegisterValueChangedCallback(changeEvent =>
            {
                fieldDuration.SetEnabled(changeEvent.newValue);
            });
            
            fieldDuration.SetEnabled(hasDuration.boolValue);

            return root;
        }
    }
}