using GameCreator.Editor.Common;
using GameCreator.Runtime.Cameras;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Cameras
{
    [CustomPropertyDrawer(typeof(CameraAvoidClip))]
    public class CameraAvoidClipDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            StyleSheet[] styleSheets = StyleSheetUtils.Load();
            foreach (StyleSheet styleSheet in styleSheets)
            {
                root.styleSheets.Add(styleSheet);
            }
            
            SerializedProperty enabled = property.FindPropertyRelative("m_Enabled");
            SerializedProperty layerMask = property.FindPropertyRelative("m_LayerMask");
            
            SerializedProperty radius = property.FindPropertyRelative("m_Radius");
            SerializedProperty distance = property.FindPropertyRelative("m_MinDistance");
            SerializedProperty smoothTime = property.FindPropertyRelative("m_SmoothTime");
            
            PropertyField fieldEnabled = new PropertyField(enabled);
            PropertyField fieldLayerMask = new PropertyField(layerMask);
            PropertyField fieldRadius = new PropertyField(radius);
            PropertyField fieldDistance = new PropertyField(distance);
            PropertyField fieldSmoothTime = new PropertyField(smoothTime);
            
            Label labelTitle = new Label("Avoid Clipping");
            labelTitle.AddToClassList("gc-label-title");
            
            VisualElement content = new VisualElement();
            
            root.Add(labelTitle);
            root.Add(fieldEnabled);
            root.Add(content);

            content.Add(fieldLayerMask);
            content.Add(fieldRadius);
            content.Add(fieldDistance);
            content.Add(fieldSmoothTime);

            fieldEnabled.RegisterValueChangeCallback(changeEvent =>
            {
                enabled.serializedObject.Update();
                content.SetEnabled(enabled.boolValue); 
            });
            
            content.SetEnabled(enabled.boolValue);
            return root;
        }
    }
}