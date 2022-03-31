using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarConditions : TToolbar
    {
        public override string Tooltip => "Create a Conditions game object";
        public override int Priority => 3;
        
        protected override IIcon CreateIcon => new IconConditions(COLOR);

        public override void Run()
        {
            ConditionsEditor.CreateElement(null);
        }
    }
}