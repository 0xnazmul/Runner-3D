using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public abstract class State : ScriptableObject, IState, ISerializationCallbackReceiver
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] protected AvatarMask m_StateMask;

        [SerializeField] private EntryAnimationClip m_Entry = new EntryAnimationClip();
        [SerializeField] private ExitAnimationClip m_Exit = new ExitAnimationClip();

        // PROPERTIES: ----------------------------------------------------------------------------

        public abstract RuntimeAnimatorController StateController { get; }
        
        public AvatarMask StateMask => this.m_StateMask;
        public bool HasStateMask => this.m_StateMask != null;

        public AnimationClip EntryClip => this.m_Entry.EntryClip;
        public bool HasEntryClip => this.m_Entry.EntryClip != null;
        public AvatarMask EntryMask => this.m_Entry.EntryMask;
        
        public AnimationClip ExitClip => this.m_Exit.ExitClip;
        public bool HasExitClip => this.m_Exit.ExitClip != null;
        public AvatarMask ExitMask => this.m_Exit.ExitMask;
        
        // SERIALIZATION CALLBACKS: ---------------------------------------------------------------
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            #if UNITY_EDITOR
            
            this.BeforeSerialize();
            
            #endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            #if UNITY_EDITOR
            
            this.AfterSerialize();
            
            #endif
        }

        protected abstract void BeforeSerialize();
        protected abstract void AfterSerialize();
    }
}