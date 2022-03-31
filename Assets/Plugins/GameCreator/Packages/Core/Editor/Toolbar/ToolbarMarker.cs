using GameCreator.Editor.Common;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarMarker : TToolbar
    {
        public override string Tooltip => "Create a navigation Marker";
        public override int Priority => 7;
        
        protected override IIcon CreateIcon => new IconMarker(COLOR);

        public override void Run()
        {
            MarkerEditor.CreateElement(null);
        }
    }
}