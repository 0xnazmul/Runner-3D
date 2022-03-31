using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    public static class EditorSettingsCore
    {
        private const string LABEL = "Core";

        // PROPERTIES: ----------------------------------------------------------------------------

        private static float MarkerCapsuleHeight
        {
            get => EditorPrefs.GetFloat(Marker.KEY_MARKER_CAPSULE_HEIGHT, 2f);
            set => EditorPrefs.SetFloat(Marker.KEY_MARKER_CAPSULE_HEIGHT, value);
        }
        
        private static float MarkerCapsuleRadius
        {
            get => EditorPrefs.GetFloat(Marker.KEY_MARKER_CAPSULE_RADIUS, 0.2f);
            set => EditorPrefs.SetFloat(Marker.KEY_MARKER_CAPSULE_RADIUS, value);
        }
        
        // REGISTRATION METHODS: ------------------------------------------------------------------
        
        [SettingsProvider]
        private static SettingsProvider CreateEditorSettingsCore()
        {
            return EditorSettingsRegistrar.CreateSettings(LABEL, CreateContent);
        }

        [InitializeOnLoadMethod]
        private static void RegisterSettings()
        {
            EditorSettingsRegistrar.RegisterSettings(LABEL);
        }
        
        // CONTENT: -------------------------------------------------------------------------------

        private static void CreateContent(string search, VisualElement content)
        {
            content.Add(new LabelTitle("Marker Gizmos"));
            FloatField fieldHeight = new FloatField("Height") { value = MarkerCapsuleHeight };
            FloatField fieldRadius = new FloatField("Radius") { value = MarkerCapsuleRadius };

            fieldHeight.RegisterValueChangedCallback(changeEvent => MarkerCapsuleHeight = changeEvent.newValue);
            fieldRadius.RegisterValueChangedCallback(changeEvent => MarkerCapsuleRadius = changeEvent.newValue);
            
            content.Add(fieldHeight);
            content.Add(fieldRadius);
        }
    }
}