using System;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class EntryAnimationClip
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private AnimationClip m_EntryClip;
        [SerializeField] private AvatarMask m_EntryMask;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public AnimationClip EntryClip => m_EntryClip;
        public AvatarMask EntryMask => m_EntryMask;
    }
}