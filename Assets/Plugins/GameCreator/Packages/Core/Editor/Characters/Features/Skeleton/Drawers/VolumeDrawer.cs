using GameCreator.Editor.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    public abstract class VolumeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty bone = property.FindPropertyRelative("m_Bone");
            SerializedProperty joint = property.FindPropertyRelative("m_Joint");
            
            PropertyField boneField = new PropertyField(bone);
            PropertyElement jointSelector = new PropertyElement(joint, joint.displayName, false);
            
            root.Add(boneField);
            this.CreateGUI(property, root);
            root.Add(jointSelector);

            return root;
        }
        
        // ABSTRACT METHODS: ----------------------------------------------------------------------

        protected abstract void CreateGUI(SerializedProperty property, VisualElement root);
    }
}