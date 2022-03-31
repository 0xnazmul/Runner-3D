using System.Threading.Tasks;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    [HelpURL("https://docs.gamecreator.io/gamecreator/visual-scripting/actions")]
    [AddComponentMenu("Game Creator/Interaction/Actions")]
    
    [Icon(RuntimePaths.GIZMOS + "GizmoActions.png")]
    public class Actions : BaseActions
    {
        public async Task Run()
        {
            await this.ExecInstructions();
        }

        public async Task Run(Args args)
        {
            await this.ExecInstructions(args);
        }

        public void Cancel()
        {
            this.StopExecInstructions();
        }
    }
}