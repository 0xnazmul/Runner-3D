using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomPropertyDrawer(typeof(FieldSetLocalList))]
    public class FieldSetLocalListDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty propertyVariable = property.FindPropertyRelative("m_Variable");
            SerializedProperty propertySelect = property.FindPropertyRelative("m_Select");

            PropertyField fieldVariable = new PropertyField(propertyVariable);
            PickFieldElement fieldSelect = new PickFieldElement(propertySelect, " ");
            
            root.Add(fieldVariable);
            root.Add(fieldSelect);

            return root;
        }
    }
}