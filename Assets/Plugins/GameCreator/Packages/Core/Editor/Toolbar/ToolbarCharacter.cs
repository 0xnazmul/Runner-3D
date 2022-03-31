using GameCreator.Editor.Characters;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarCharacter : TToolbar
    {
        public override string Tooltip => "Create a Character";
        public override int Priority => 5;
            
        protected override IIcon CreateIcon => new IconCharacter(COLOR);

        // METHODS: -------------------------------------------------------------------------------
        
        public override void Run()
        {
            CharacterEditor.CreateCharacter(null);
        }
    }
}