using GameCreator.Runtime.Cameras;
using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Cameras
{
    [CustomEditor(typeof(TCamera), true)]
    public class TCameraEditor : UnityEditor.Editor
    {
        // CREATE INSPECTOR: ----------------------------------------------------------------------

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty timeMode = this.serializedObject.FindProperty("m_TimeMode");
            SerializedProperty runIn = this.serializedObject.FindProperty("m_RunIn");
            SerializedProperty transition = this.serializedObject.FindProperty("m_Transition");
            SerializedProperty avoidClip = this.serializedObject.FindProperty("m_AvoidClip");
            
            PropertyField fieldTimeMode = new PropertyField(timeMode);
            PropertyField fieldRunIn = new PropertyField(runIn);
            PropertyField fieldTransition = new PropertyField(transition);
            PropertyField fieldAvoidClip = new PropertyField(avoidClip);
            
            root.Add(fieldTimeMode);
            root.Add(fieldRunIn);
            root.Add(fieldTransition);
            root.Add(fieldAvoidClip);

            return root;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/Cameras/Main Camera", false, 0)]
        private static void CreateElement(MenuCommand menuCommand)
        {
            GameObject instance = new GameObject("Main Camera");
            instance.AddComponent<Camera>();
            instance.AddComponent<AudioListener>();
            instance.AddComponent<PhysicsRaycaster>();
            instance.AddComponent<MainCamera>();
            instance.tag = "MainCamera";
            
            GameObjectUtility.SetParentAndAlign(instance, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}
