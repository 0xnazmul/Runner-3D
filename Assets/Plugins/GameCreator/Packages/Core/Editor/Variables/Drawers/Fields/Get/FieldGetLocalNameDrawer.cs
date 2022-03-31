using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Variables
{
    [CustomPropertyDrawer(typeof(FieldGetLocalName))]
    public class FieldGetLocalNameDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            SerializedProperty variable = property.FindPropertyRelative("m_Variable");
            SerializedProperty typeID = property.FindPropertyRelative("m_TypeID");

            ObjectField fieldVariable = new ObjectField(variable.displayName)
            {
                allowSceneObjects = true,
                objectType = typeof(LocalNameVariables),
                bindingPath = variable.propertyPath
            };
            
            SerializedProperty typeIDStr = typeID.FindPropertyRelative(IdStringDrawer.NAME_STRING);
            IdString typeIDValue = new IdString(typeIDStr.stringValue);
            
            LocalNamePickTool toolPickName = new LocalNamePickTool(
                fieldVariable, 
                property,
                typeIDValue
            );

            root.Add(fieldVariable);
            root.Add(toolPickName);

            return root;
        }
    }
}