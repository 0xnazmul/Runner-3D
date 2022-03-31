using UnityEditor;
using UnityEngine.UIElements;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Characters
{
    [CustomPropertyDrawer(typeof(IUnitMotion), true)]
    public class IUnitMotionDrawer : TUnitDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return this.MakePropertyGUI(property, "Motion");
        }
        
        protected override IIcon UnitIcon => new IconChip(ColorTheme.Type.TextLight);
    }
}