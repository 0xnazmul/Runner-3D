using GameCreator.Editor.Variables;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarLocalListVariables : TToolbar
    {
        public override string Tooltip => "Create a Local List Variables";
        public override int Priority => 11;
        
        protected override IIcon CreateIcon => new IconListVariable(COLOR);

        // METHODS: -------------------------------------------------------------------------------
        
        public override void Run()
        {
            LocalListVariablesEditor.CreateLocalListVariables(null);
        }
    }
}