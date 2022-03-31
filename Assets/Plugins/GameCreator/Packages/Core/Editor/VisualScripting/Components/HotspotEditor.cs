using UnityEditor;
using GameCreator.Runtime.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace GameCreator.Editor.VisualScripting
{
    [CustomEditor(typeof(Hotspot))]
    public class HotspotEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();

            container.Add(new PropertyField(this.serializedObject.FindProperty("m_Target")));
            
            container.Add(new PropertyField(this.serializedObject.FindProperty("m_Radius")));
            container.Add(new PropertyField(this.serializedObject.FindProperty("m_Offset")));

            container.Add(new PropertyField(this.serializedObject.FindProperty("m_Spots")));

            return container;
        }
        
        // CREATION MENU: -------------------------------------------------------------------------
        
        [MenuItem("GameObject/Game Creator/Interaction/Hotspot", false, 0)]
        public static void CreateElement(MenuCommand menuCommand)
        {
            GameObject instance = new GameObject("Hotspot");
            instance.AddComponent<Hotspot>();
            
            GameObjectUtility.SetParentAndAlign(instance, menuCommand?.context as GameObject);

            Undo.RegisterCreatedObjectUndo(instance, $"Create {instance.name}");
            Selection.activeObject = instance;
        }
    }
}