using GameCreator.Editor.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Cameras
{
    public abstract class TShotSystemDrawer : TBoxDrawer
    {
        public const string PROP_IS_ACTIVE = "m_IsActive";

        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        protected override void CreatePropertyContent(VisualElement root, SerializedProperty property)
        {
            SerializedProperty isActive = property.FindPropertyRelative(PROP_IS_ACTIVE);
            PropertyField fieldIsActive = new PropertyField(isActive);
            VisualElement content = new VisualElement();
            
            fieldIsActive.RegisterValueChangeCallback(changeEvent =>
            {
                RefreshContent(content, property);
            });
            
            RefreshContent(content, property);
            
            root.Add(fieldIsActive);
            root.Add(content);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void RefreshContent(VisualElement content, SerializedProperty property)
        {
            SerializedProperty isActive = property.FindPropertyRelative(PROP_IS_ACTIVE);
            
            content.Clear();
            content.SetEnabled(isActive.boolValue);
            
            SerializationUtils.CreateChildProperties(
                content,
                property, 
                false,
                PROP_IS_ACTIVE
            );
        }
    }
}