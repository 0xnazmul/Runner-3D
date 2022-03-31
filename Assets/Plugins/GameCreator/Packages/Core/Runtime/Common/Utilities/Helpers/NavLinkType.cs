using System;
using UnityEngine;
using UnityEngine.AI;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class NavLinkType
    {
        public enum LinkType
        {
            Manual = OffMeshLinkType.LinkTypeManual,
            Drop = OffMeshLinkType.LinkTypeDropDown,
            Jump = OffMeshLinkType.LinkTypeJumpAcross
        }
        
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private LinkType m_LinkType = LinkType.Jump;
        [SerializeField] private NavAreaMask m_ForAreas = new NavAreaMask();
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public bool Match(NavMeshAgent agent)
        {
            if (agent == null) return false;
            if (!agent.isOnOffMeshLink) return false;
            
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            if (!data.valid) return false;

            if (data.linkType != (OffMeshLinkType) this.m_LinkType) return false;
            if (data.linkType != OffMeshLinkType.LinkTypeManual) return true;
            
            if (data.offMeshLink == null) return false;
            return (1 << data.offMeshLink.area & this.m_ForAreas.Mask) > 1;
        }
    }
}