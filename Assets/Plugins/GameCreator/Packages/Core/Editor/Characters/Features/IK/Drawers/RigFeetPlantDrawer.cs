using GameCreator.Runtime.Characters.IK;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(RigFeetPlant))]
    public class RigFeetPlantDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty footOffset = property.FindPropertyRelative("m_FootOffset");
            SerializedProperty footMask = property.FindPropertyRelative("m_FootMask");
            SerializedProperty alignHips = property.FindPropertyRelative("m_AlignHips");
            
            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(footOffset));
            root.Add(new PropertyField(footMask));
            root.Add(new PropertyField(alignHips));

            return root;
        }
    }
}