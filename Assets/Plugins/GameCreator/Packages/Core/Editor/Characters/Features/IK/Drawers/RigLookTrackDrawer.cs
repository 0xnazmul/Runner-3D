using GameCreator.Runtime.Characters.IK;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(RigLookTrack))]
    public class RigLookTrackDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty trackSpeed = property.FindPropertyRelative("m_TrackSpeed");
            SerializedProperty maxAngle = property.FindPropertyRelative("m_MaxAngle");
            
            SerializedProperty headWeight = property.FindPropertyRelative("m_HeadWeight");
            SerializedProperty neckWeight = property.FindPropertyRelative("m_NeckWeight");
            SerializedProperty chestWeight = property.FindPropertyRelative("m_ChestWeight");
            SerializedProperty spineWeight = property.FindPropertyRelative("m_SpineWeight");
            
            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(trackSpeed));
            root.Add(new PropertyField(maxAngle));
            
            root.Add(new PropertyField(headWeight));
            root.Add(new PropertyField(neckWeight));
            root.Add(new PropertyField(chestWeight));
            root.Add(new PropertyField(spineWeight));

            return root;
        }
    }
}