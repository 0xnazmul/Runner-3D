using GameCreator.Editor.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomPropertyDrawer(typeof(ListVariableRuntime))]
    public class ListVariablesRuntimeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty list = property.FindPropertyRelative("m_List");
            ListVariableRuntime runtime = property.GetValue<ListVariableRuntime>();

            switch (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                case true:
                    root.Add(new IndexListView(runtime));
                    break;
                
                case false:
                    root.Add(new IndexListTool(list));
                    break;
            }

            return root;
        }
    }
}