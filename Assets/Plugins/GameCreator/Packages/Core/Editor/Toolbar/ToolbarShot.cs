using GameCreator.Editor.Cameras;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarShot : TToolbar
    {
        public override string Tooltip => "Create a Camera Shot";
        public override int Priority => 9;
            
        protected override IIcon CreateIcon => new IconCameraShot(COLOR);

        // METHODS: -------------------------------------------------------------------------------
        
        public override void Run()
        {
            ShotCameraEditor.CreateElement(null);
        }
    }
}