using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomEditor(typeof(LocalListVariables))]
    public class LocalListVariablesEditor : UnityEditor.Editor
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

        [MenuItem("GameObject/Game Creator/Variables/List Variables", false, 0)]
        public static void CreateLocalListVariables(MenuCommand menuCommand)
        {
            GameObject instance = new GameObject("List Variables");
            instance.AddComponent<LocalListVariables>();

            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}
