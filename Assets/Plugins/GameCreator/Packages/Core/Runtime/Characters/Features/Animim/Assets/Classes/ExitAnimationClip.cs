using System;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    [Serializable]
    public class ExitAnimationClip
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        [SerializeField] private AnimationClip m_ExitClip;
        [SerializeField] private AvatarMask m_ExitMask;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public AnimationClip ExitClip => m_ExitClip;
        public AvatarMask ExitMask => m_ExitMask;
    }
}