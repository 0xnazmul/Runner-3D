using GameCreator.Editor.Characters;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarPlayer : TToolbar
    {
        public override string Tooltip => "Create a Player";
        public override int Priority => 6;
            
        protected override IIcon CreateIcon => new IconPlayer(COLOR);

        // METHODS: -------------------------------------------------------------------------------
        
        public override void Run()
        {
            CharacterEditor.CreatePlayer(null);
        }
    }
}