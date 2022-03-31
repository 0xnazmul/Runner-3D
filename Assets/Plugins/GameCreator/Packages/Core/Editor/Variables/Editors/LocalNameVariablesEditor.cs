using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomEditor(typeof(LocalNameVariables))]
    public class LocalNameVariablesEditor : UnityEditor.Editor
    {
        // PAINT METHOD: --------------------------------------------------------------------------
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            root.style.marginTop = new StyleLength(5);

            SerializedProperty runtime = this.serializedObject.FindProperty("m_Runtime");
            SerializedProperty saveUniqueID = this.serializedObject.FindProperty("m_SaveUniqueID");

            PropertyField fieldRuntime = new PropertyField(runtime);
            PropertyField fieldSaveUniqueID = new PropertyField(saveUniqueID);

            root.Add(fieldRuntime);
            root.Add(fieldSaveUniqueID);

            return root;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------

        [MenuItem("GameObject/Game Creator/Variables/Name Variables", false, 0)]
        public static void CreateLocalNameVariables(MenuCommand menuCommand)
        {
            GameObject instance = new GameObject("Name Variables");
            instance.AddComponent<LocalNameVariables>();

            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}
