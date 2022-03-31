using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GameCreator.Runtime.Characters.Animim
{
    public class StatePlayableBehaviour : TAnimimPlayableBehaviour
    {
        private const float TRANSITION_FAST = 0.15f;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly AnimationClip m_ExitClip;
        private readonly AvatarMask m_ExitMask;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public int Layer { get; }
        
        public bool IsEntryClipComplete { get; private set; }
        public bool IsExitClipComplete { get; private set; }

        protected override Playable AnimationPlayable { get; }

        // CONSTRUCTORS: --------------------------------------------------------------------------
        
        public StatePlayableBehaviour(AnimationClip animationClip, AvatarMask avatarMask, int layer,
            BlendMode blendMode, AnimimGraph animimGraph, ConfigState config) 
            : base(avatarMask, blendMode, animimGraph, config)
        {
            this.Layer = layer;
            this.IsEntryClipComplete = true;
            this.IsExitClipComplete = false;

            this.AnimationPlayable = AnimationClipPlayable.Create(
                animimGraph.Graph, 
                animationClip
            );
        }
        
        public StatePlayableBehaviour(RuntimeAnimatorController rtc, AvatarMask avatarMask, 
            int layer, BlendMode blendMode, AnimimGraph animimGraph, ConfigState config) 
            : base(avatarMask, blendMode, animimGraph, config)
        {
            this.Layer = layer;
            this.IsEntryClipComplete = true;
            this.IsExitClipComplete = false;

            this.AnimationPlayable = AnimatorControllerPlayable.Create(
                animimGraph.Graph, 
                rtc
            );
        }
        
        public StatePlayableBehaviour(State state, int layer, BlendMode blendMode, 
            AnimimGraph animimGraph, ConfigState config) 
            : base(state.StateMask, blendMode, animimGraph, config)
        {
            this.IsEntryClipComplete = true;
            this.IsExitClipComplete = false;

            if (state.HasEntryClip)
            {
                this.IsEntryClipComplete = false;
                _ = this.PlayEntryClip(animimGraph, state, config);
                
                this.m_Config.TransitionIn = 0f;
                this.m_Config.DelayIn += config.TransitionIn;
            }
            
            if (state.HasExitClip)
            {
                this.m_ExitClip = state.ExitClip;
                this.m_ExitMask = state.ExitMask;
            }
            
            this.Layer = layer;

            this.AnimationPlayable = AnimatorControllerPlayable.Create(
                animimGraph.Graph, 
                state.StateController
            );
        }
        
        public StatePlayableBehaviour() : base(null, BlendMode.Blend, null, default)
        { }

        public override void Stop(float delay, float transitionOut)
        {
            if (this.m_ExitClip != null)
            {
                _ = this.PlayExitClip(new ConfigGesture(
                    delay, this.m_ExitClip.length, 1f, false,
                    TRANSITION_FAST, transitionOut
                ));

                delay += TRANSITION_FAST;
                transitionOut = 0f;
            }
            
            base.Stop(delay, transitionOut);
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private async Task PlayEntryClip(AnimimGraph animimGraph, State state, ConfigState config)
        {
            await animimGraph.Gestures.CrossFade(
                state.EntryClip, state.StateMask, this.m_BlendMode,
                new ConfigGesture(
                    config.DelayIn, state.EntryClip.length,
                    1f, config.RootMotion,
                    config.TransitionIn, TRANSITION_FAST
                ),
                false
            );

            this.IsEntryClipComplete = true;
        }
        
        private async Task PlayExitClip(ConfigGesture config)
        {
            await this.m_AnimimGraph.Gestures.CrossFade(
                this.m_ExitClip, this.m_ExitMask, this.m_BlendMode,
                config, false
            );

            this.IsExitClipComplete = true;
        }
    }
}