using GameCreator.Editor.Common;
using GameCreator.Runtime.Characters;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(Footsteps))]
    public class FootstepsDrawer : TSectionDrawer
    {
        protected override string Name(SerializedProperty property) => "Footsteps";

        protected override void CreatePropertyContent(VisualElement container, 
            SerializedProperty property)
        {
            FeetTool feetTool = new FeetTool(property);
            SerializedProperty groundThreshold = property.FindPropertyRelative("m_GroundThreshold");
            SerializedProperty footstepSounds = property.FindPropertyRelative("m_FootstepSounds");
            
            container.Add(feetTool);
            container.Add(new PropertyField(groundThreshold));
            container.Add(new PropertyField(footstepSounds));
        }
    }
}
