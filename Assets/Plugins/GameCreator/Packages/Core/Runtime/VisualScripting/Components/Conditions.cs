using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.VisualScripting
{
    [HelpURL("https://docs.gamecreator.io/gamecreator/visual-scripting/conditions")]
    [AddComponentMenu("Game Creator/Interaction/Conditions")]
    
    [Icon(RuntimePaths.GIZMOS + "GizmoConditions.png")]
    public class Conditions : MonoBehaviour
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField]
        protected BranchList m_Branches = new BranchList();
        
        private Args m_Args;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsRunning => this.m_Branches.IsRunning;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public async Task Run()
        {
            this.m_Args ??= new Args(this.gameObject);
            await this.Run(this.m_Args);
        }

        public async Task Run(Args args)
        {
            await this.m_Branches.Evaluate(args);
        }

        public void Cancel()
        {
            this.m_Branches.Cancel();
        }
    }
}
