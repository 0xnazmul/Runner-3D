using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class Interactions
    {
        // TODO: Create an interaction system
        // How to identify each key with a unique ID that can work with the Input System?
        // The higher the priority of an IInteractive the more likely it will be chosen as
        // the selected interactive element
        
        // MEMBERS: -------------------------------------------------------------------------------

        private Dictionary<int, List<IInteractive>> m_Interactions;
        
        // CONSTRUCTOR: ---------------------------------------------------------------------------

        public Interactions()
        {
            this.m_Interactions = new Dictionary<int, List<IInteractive>>();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Register(IInteractive interactive)
        {
            if (interactive == null) return;
            
            if (!this.m_Interactions.TryGetValue(interactive.ID, out var list))
            {
                list = new List<IInteractive>();
                this.m_Interactions.Add(interactive.ID, list);
            }
            
            list.Add(interactive);
        }
        
        public void Unregister(IInteractive interactive)
        {
            if (interactive == null) return;
            this.m_Interactions.Remove(interactive.ID);
        }
    }
}
