using GameCreator.Editor.VisualScripting;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarHotspot : TToolbar
    {
        public override string Tooltip => "Create a Hotspot";
        public override int Priority => 8;
        
        protected override IIcon CreateIcon => new IconHotspot(COLOR);

        public override void Run()
        {
            HotspotEditor.CreateElement(null);
        }
    }
}