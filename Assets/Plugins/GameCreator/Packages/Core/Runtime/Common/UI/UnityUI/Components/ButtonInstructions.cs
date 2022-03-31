using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GameCreator.Runtime.Common.UnityUI
{
    [AddComponentMenu("Game Creator/UI/Button")]
    [RequireComponent(typeof(Image))]
    [Icon(RuntimePaths.GIZMOS + "GizmoUIButton.png")]
    public class ButtonInstructions : Button
    {
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private InstructionList m_Instructions = new InstructionList(
            new InstructionCommonDebugText("Click!")
        );
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Args m_Args;
        
        // INIT METHODS: --------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying) return;

            this.m_Args = new Args(gameObject);
            this.onClick.AddListener(() => _ = this.m_Instructions.Run(this.m_Args));
        }
    }
}
