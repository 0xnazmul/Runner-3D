using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace GameCreator.Runtime.Common.Audio
{
    public abstract class TAudioChannel : IAudioChannel
    {
        private const int ALLOCATE_BUFFER_BLOCK = 5;
        
        // MEMBERS: -------------------------------------------------------------------------------

        private readonly Transform m_Parent;
        
        private readonly Queue<AudioBuffer> m_AvailableBuffers = new Queue<AudioBuffer>();
        private readonly List<AudioBuffer> m_ActiveBuffers = new List<AudioBuffer>();
        
        // PROPERTIES: ----------------------------------------------------------------------------
        
        protected abstract float Volume { get; }

        protected abstract AudioMixerGroup AudioOutput { get; }
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected TAudioChannel(Transform parent)
        {
            this.m_Parent = parent;
        }
        
        // UPDATE METHOD: -------------------------------------------------------------------------
        
        internal void Update()
        {
            for (int i = this.m_ActiveBuffers.Count - 1; i >= 0; --i)
            {
                AudioBuffer activeBuffer = this.m_ActiveBuffers[i];
                if (!activeBuffer.Update(AudioManager.Instance.Volume.Master * this.Volume))
                {
                    this.m_ActiveBuffers.RemoveAt(i);
                    this.m_AvailableBuffers.Enqueue(activeBuffer);
                }
            }
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool IsPlaying(AudioClip audioClip)
        {
            foreach (AudioBuffer activeBuffer in this.m_ActiveBuffers)
            {
                if (activeBuffer.AudioClip == audioClip) return true;
            }

            return false;
        }
        
        public async Task Play(AudioClip audioClip, IAudioConfig audioConfig, Args args)
        {
            if (this.m_AvailableBuffers.Count == 0) this.AllocateAudioBuffers();

            AudioBuffer audioBuffer = this.m_AvailableBuffers.Dequeue();
            this.m_ActiveBuffers.Add(audioBuffer);

            await audioBuffer.Play(audioClip, audioConfig, args);
        }

        public async Task Stop(AudioClip audioClip, float transitionOut)
        {
            List<Task> tasks = new List<Task>();
            foreach (AudioBuffer activeBuffer in this.m_ActiveBuffers)
            {
                if (activeBuffer.AudioClip != audioClip) continue;
                tasks.Add(activeBuffer.Stop(transitionOut));
            }

            await Task.WhenAll(tasks);
        }
        
        public async Task Stop(GameObject target, float transitionOut)
        {
            if (target == null) return;
            
            List<Task> tasks = new List<Task>();
            foreach (AudioBuffer activeBuffer in this.m_ActiveBuffers)
            {
                if (activeBuffer.Target != target) continue;
                tasks.Add(activeBuffer.Stop(transitionOut));
            }

            await Task.WhenAll(tasks);
        }

        public async Task StopAll(float transition)
        {
            List<Task> tasks = new List<Task>();
            foreach (AudioBuffer activeBuffer in this.m_ActiveBuffers)
            {
                tasks.Add(activeBuffer.Stop(transition));
            }

            await Task.WhenAll(tasks);
        }
        
        // INTERNAL METHODS: ----------------------------------------------------------------------

        internal void OnExit()
        {
            this.m_AvailableBuffers.Clear();
            this.m_ActiveBuffers.Clear();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void AllocateAudioBuffers()
        {
            for (int i = 0; i < ALLOCATE_BUFFER_BLOCK; ++i)
            {
                AudioBuffer audioBuffer = this.MakeAudioBuffer();
                this.m_AvailableBuffers.Enqueue(audioBuffer);
            }
        }
        
        // VIRTUAL METHODS: -----------------------------------------------------------------------

        protected virtual AudioBuffer MakeAudioBuffer()
        {
            return new AudioBuffer(this.m_Parent, this.AudioOutput);
        }
        
        
    }
}