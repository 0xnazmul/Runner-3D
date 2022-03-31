using System.Threading.Tasks;
using UnityEngine;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.VisualScripting
{
    public abstract class BaseActions : MonoBehaviour
    {
        protected static BaseActions ExecutingInForeground;

        protected static bool IsForegroundRunning => ExecutingInForeground != null;
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField]
        protected InstructionList m_Instructions = new InstructionList();

        [SerializeField]
        protected bool m_InBackground = true;

        private Args m_Args;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsRunning => this.m_Instructions.IsRunning;

        // PROTECTED METHODS: ---------------------------------------------------------------------

        protected async Task ExecInstructions()
        {
            this.m_Args ??= new Args(this.gameObject);
            await this.ExecInstructions(this.m_Args);
        }

        protected async Task ExecInstructions(Args args)
        {
            if (!this.m_InBackground && IsForegroundRunning) return;

            ExecutingInForeground = this;
            await this.m_Instructions.Run(args);
            ExecutingInForeground = null;
        }

        protected void StopExecInstructions()
        {
            this.m_Instructions.Cancel();
        }

        // BEHAVIOR METHODS: ----------------------------------------------------------------------

        protected virtual void OnDisable()
        {
            this.StopExecInstructions();
        }
    }
}