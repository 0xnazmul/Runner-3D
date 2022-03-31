using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarActions : TToolbar
    {
        public override string Tooltip => "Create an Actions game object";
        public override int Priority => 4;
        
        protected override IIcon CreateIcon => new IconInstructions(COLOR);

        public override void Run()
        {
            ActionsEditor.CreateElement(null);
        }
    }
}
