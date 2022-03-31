using System;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace GameCreator.Runtime.Characters.Animim
{
    public abstract class TAnimimPlayableBehaviour : PlayableBehaviour
    {
        // FIELDS: --------------------------------------------------------------------------------
        
        public Playable behaviourPlayable;
        public AnimationLayerMixerPlayable mixerPlayable;
        
        // MEMBERS: -------------------------------------------------------------------------------
        
        protected readonly AvatarMask m_AvatarMask;
        protected readonly BlendMode m_BlendMode;
        
        protected readonly AnimimGraph m_AnimimGraph;
        protected readonly IConfig m_Config;

        private TAnimimOutput m_ParentOutput;

        // PROPERTIES: ----------------------------------------------------------------------------

        protected abstract Playable AnimationPlayable { get; }
        
        protected AnimFloat Weight { get; }

        protected float StartTime { get; }
        protected float CurrentTime { get; private set; }

        public bool IsComplete { get; private set; }

        public float RootMotion => this.m_Config?.RootMotion ?? false 
            ? this.Weight.Current 
            : 0f; 

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TAnimimPlayableBehaviour(AvatarMask avatarMask, BlendMode blendMode, 
            AnimimGraph animimGraph, IConfig config)
        {
            this.m_AvatarMask = avatarMask;
            this.m_BlendMode = blendMode;
            
            this.m_AnimimGraph = animimGraph;
            this.m_Config = config;

            this.StartTime = animimGraph.Character.Time.Time;
            
            this.Weight = new AnimFloat(0f, this.m_Config.TransitionIn);
        }

        // OVERRIDE METHODS: ----------------------------------------------------------------------
        
        public override void OnPlayableCreate(Playable playable)
        {
            base.OnPlayableCreate(playable);
            this.behaviourPlayable = playable;
            
            if (this.m_Config.Duration > float.Epsilon)
            {
                float totalDuration = this.m_Config.DelayIn + this.m_Config.Duration; 
                playable.SetDuration(totalDuration);
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);

            this.CurrentTime = (float) playable.GetTime();
            
            this.UpdateFrame();
            
            playable.GetInput(0).SetInputWeight(1, this.Weight.Current);

            if (playable.IsDone())
            {
                Playable mixer = playable.GetInput(0);
                Playable source = mixer.GetInput(0);
                Playable parent = playable.GetOutput(0);
                
                mixer.DisconnectInput(0);
                parent.DisconnectInput(0);
                
                parent.ConnectInput(0, source, 0);
                parent.SetInputWeight(0, 1f);
                
                playable.Destroy();
                this.m_ParentOutput.OnDeleteChild(this);
                this.IsComplete = true;
            }
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void UpdateFrame()
        {
            float elapsedTime = this.m_AnimimGraph.Character.Time.Time - this.StartTime;
            
            if (elapsedTime < this.m_Config.DelayIn)
            {
                this.AnimationPlayable.Pause();
            }
            else
            {
                this.AnimationPlayable.Play();
                
                this.Weight.Target = this.m_Config.Weight;
                this.Weight.Smooth = this.m_Config.TransitionIn;
            }

            if (this.m_Config.Duration > float.Epsilon)
            {
                float timeToFadeout = Math.Max(
                    this.m_Config.Duration - this.m_Config.TransitionOut, 
                    this.m_Config.TransitionIn
                );
                
                if (elapsedTime >= this.m_Config.DelayIn + timeToFadeout)
                {
                    this.Weight.Target = 0f;
                    this.Weight.Smooth = this.m_Config.TransitionOut;
                }
            }
            
            this.Weight.Update();
        }

        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Create(TAnimimOutput parentOutput)
        {
            this.m_ParentOutput = parentOutput;
            
            Playable source = this.behaviourPlayable.GetInput(0);
            this.behaviourPlayable.DisconnectInput(0);
            
            this.mixerPlayable = AnimationLayerMixerPlayable.Create(
                this.m_AnimimGraph.Graph, 2
            );
            
            this.mixerPlayable.ConnectInput(0, source, 0);
            this.mixerPlayable.ConnectInput(1, this.AnimationPlayable, 0);
            
            this.behaviourPlayable.ConnectInput(0, this.mixerPlayable, 0);
            this.behaviourPlayable.SetInputWeight(0, 1f);

            if (this.m_AvatarMask != null)
            {
                this.mixerPlayable.SetLayerMaskFromAvatarMask(1, this.m_AvatarMask);
            }
            
            this.mixerPlayable.SetLayerAdditive(1, this.m_BlendMode == BlendMode.Additive);
            
            this.mixerPlayable.SetInputWeight(0, 1f);
            this.mixerPlayable.SetInputWeight(1, 0f);
            
            this.AnimationPlayable.SetSpeed(this.m_Config.Speed);
        }

        public void Stop()
        {
            this.Stop(0f, this.m_Config.TransitionOut);
        }

        public virtual void Stop(float delay, float transitionOut)
        {
            float newDuration = this.CurrentTime + delay + transitionOut;
            this.behaviourPlayable.SetDuration(newDuration);

            this.m_Config.DelayIn = 0f;
            this.m_Config.Duration = newDuration;
            this.m_Config.TransitionOut = transitionOut;
        }

        public void ChangeWeight(float weight, float transition)
        {
            this.m_Config.Weight = weight;
            this.m_Config.TransitionIn = transition;
        }
    }
}