using GameCreator.Editor.Variables;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    internal class ToolbarLocalNameVariables : TToolbar
    {
        public override string Tooltip => "Create a Local Name Variables";
        public override int Priority => 10;
            
        protected override IIcon CreateIcon => new IconNameVariable(COLOR);

        // METHODS: -------------------------------------------------------------------------------
        
        public override void Run()
        {
            LocalNameVariablesEditor.CreateLocalNameVariables(null);
        }
    }
}