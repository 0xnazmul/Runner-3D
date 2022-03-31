using UnityEditor;
using UnityEngine.UIElements;
using GameCreator.Runtime.VisualScripting;

namespace GameCreator.Editor.VisualScripting
{
    [CustomPropertyDrawer(typeof(ConditionList))]
    public class ConditionListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            ConditionListTool conditionListTool = new ConditionListTool(
                property
            );

            return conditionListTool;
        }
    }
}