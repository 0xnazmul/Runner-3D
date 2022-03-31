using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GameCreator.Runtime.Characters.Animim
{
    [Serializable]
    public class AnimimGraph
    {
        private const string NAME_GRAPH = "Animim Graph";
        private const string NAME_ANIM_OUTPUT = "Animation Output";
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        //                                                                                       //
        //     +--------+   +----+   +----------+   +--------+   +---------------------+         //
        // <===| OUTPUT |===| IK |===| GESTURES |===| STATES |===| ANIMATOR CONTROLLER |         //
        //     +--------+   +----+   +----------+   +--------+   +---------------------+         //
        //                                                                                       //
        ///////////////////////////////////////////////////////////////////////////////////////////

        // MEMBERS: -------------------------------------------------------------------------------

        private Character m_Character;
        private AnimationPlayableOutput m_AnimationOutput;
        private AnimationLayerMixerPlayable m_IK;
        
        private ScriptPlayable<StatesOutput> m_States;
        private ScriptPlayable<GesturesOutput> m_Gestures;

        private RuntimeAnimatorController m_RuntimeController;

        // PROPERTIES: ----------------------------------------------------------------------------

        internal Character Character => this.m_Character;
        
        internal PlayableGraph Graph { get; private set; }

        public StatesOutput States => this.m_States.IsValid() 
            ? this.m_States.GetBehaviour() 
            : null;
        
        public GesturesOutput Gestures => this.m_Gestures.IsValid() 
            ? this.m_Gestures.GetBehaviour() 
            : null;

        /// <summary>
        /// Returns a value between 0 and 1, where 0 means there isn't a single animation using
        /// root motion and 1 meaning there's at least one animation using it
        /// </summary>
        internal float RootMotion => Math.Max(this.Gestures.RootMotion, this.States.RootMotion);

        // PUBLIC METHODS: ------------------------------------------------------------------------

        internal void OnStartup(Character character)
        {
            this.m_Character = character;
            this.CreateGraph(this.m_Character.Animim.Animator);
            
            character.EventAfterChangeModel += this.OnModelChange;
        }
        
        internal void AfterStartup(Character character)
        { }

        internal void OnDispose(Character character)
        {
            character.EventAfterChangeModel -= this.OnModelChange;
            this.DestroyGraph();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------
        
        private void CreateGraph(Animator animator)
        {
            if (animator == null)
            {
                Debug.LogError("Animator reference is null");
                return;
            }

            if (this.Graph.IsValid()) this.Graph.Destroy();
            this.Graph = PlayableGraph.Create(NAME_GRAPH);
            
            this.m_AnimationOutput = AnimationPlayableOutput.Create(
                this.Graph, NAME_ANIM_OUTPUT, 
                animator
            );

            this.SetIK();

            StatesOutput templateStates = new StatesOutput(this);
            GesturesOutput templateGestures = new GesturesOutput(this);

            this.m_States = ScriptPlayable<StatesOutput>.Create(this.Graph, templateStates, 1);
            this.m_Gestures = ScriptPlayable<GesturesOutput>.Create(this.Graph, templateGestures, 1);

            this.m_States.SetInputWeight(0, 1f);
            this.m_Gestures.SetInputWeight(0, 1f);

            if (animator.runtimeAnimatorController != null)
            {
                this.m_RuntimeController = animator.runtimeAnimatorController;
            }

            if (this.m_RuntimeController != null)
            {
                AnimatorControllerPlayable animatorController = AnimatorControllerPlayable.Create(
                    this.Graph, 
                    this.m_RuntimeController
                );
                
                this.Graph.Connect(animatorController, 0, this.m_States, 0);
            }

            this.Graph.Connect(this.m_States, 0, this.m_Gestures, 0);
            this.Graph.Connect(this.m_Gestures, 0, this.m_IK, 0);
            
            this.m_AnimationOutput.SetSourcePlayable(this.m_IK);

            this.Graph.Play();
        }

        private void DestroyGraph()
        {
            if (!this.Graph.IsValid()) return;
            this.Graph.Destroy();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void SetIK()
        {
            AnimationClipPlayable passIK = AnimationClipPlayable.Create(this.Graph, null);
            
            passIK.SetApplyFootIK(true);
            passIK.SetApplyPlayableIK(true);

            this.m_IK = AnimationLayerMixerPlayable.Create(this.Graph, 2);
            this.m_IK.ConnectInput(1, passIK, 0);
            
            this.m_IK.SetLayerAdditive(0, false);
            this.m_IK.SetLayerAdditive(1, true);
            
            this.m_IK.SetInputWeight(0, 1f);
            this.m_IK.SetInputWeight(1, 1f);
        }
        
        // CALLBACKS: -----------------------------------------------------------------------------
        
        private void OnModelChange()
        {
            this.CreateGraph(this.m_Character.Animim.Animator);
        }
    }
}
