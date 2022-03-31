using GameCreator.Runtime.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Common.Drawers
{
    [CustomPropertyDrawer(typeof(IRepository), true)]
    public class IRepositoryDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            SerializationUtils.CreateChildProperties(root, property, false);
            
            return root;
        }
    }
}