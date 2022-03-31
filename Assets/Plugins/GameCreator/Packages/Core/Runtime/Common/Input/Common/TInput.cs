using System;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public abstract class TInput
    {
        public virtual bool Active { get; set; } = false;
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public virtual void OnStartup()
        { }
        
        public virtual void OnDispose()
        { }

        public virtual void OnUpdate()
        { }
    }
}