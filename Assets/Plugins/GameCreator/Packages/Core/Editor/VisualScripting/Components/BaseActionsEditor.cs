using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace GameCreator.Editor.VisualScripting
{
    public abstract class BaseActionsEditor : UnityEditor.Editor
    {
        protected static readonly StyleLength DEFAULT_MARGIN_TOP = new StyleLength(5);

        protected void CreateInstructionsGUI(VisualElement container)
        {
            SerializedProperty instructions = this.serializedObject.FindProperty("m_Instructions");
            SerializedProperty execBackground = this.serializedObject.FindProperty("m_InBackground");

            PropertyField fieldInstructions = new PropertyField(instructions);
            PropertyField fieldExecBackground = new PropertyField(execBackground);
            fieldExecBackground.style.marginTop = DEFAULT_MARGIN_TOP;

            container.Add(fieldInstructions);
            container.Add(fieldExecBackground);
        }
    }
}