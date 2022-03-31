using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common
{
    [CustomEditor(typeof(Remember))]
    public class RememberEditor : UnityEditor.Editor
    {
        private static readonly StyleLength DEFAULT_MARGIN_TOP = new StyleLength(5);
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement container = new VisualElement();
            container.style.marginTop = DEFAULT_MARGIN_TOP;

            SerializedProperty memories = this.serializedObject.FindProperty("m_Memories");
            SerializedProperty saveUniqueID = this.serializedObject.FindProperty("m_SaveUniqueID");

            PropertyField fieldMemories = new PropertyField(memories);
            PropertyField fieldUniqueID = new PropertyField(saveUniqueID);
            
            container.Add(fieldMemories);
            container.Add(fieldUniqueID);

            return container;
            
        }
    }
}
