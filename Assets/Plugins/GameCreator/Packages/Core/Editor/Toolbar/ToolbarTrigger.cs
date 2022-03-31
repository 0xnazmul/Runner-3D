using GameCreator.Editor.Common;
using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarTrigger : TToolbar
    {
        public override string Tooltip => "Create a Trigger game object";
        public override int Priority => 2;
        
        protected override IIcon CreateIcon => new IconTriggers(COLOR);

        public override void Run()
        {
            TriggerEditor.CreateElement(null);
        }
    }
}