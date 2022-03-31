using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class GeneralRepository : TRepository<GeneralRepository>
    {
        // REPOSITORY PROPERTIES: -----------------------------------------------------------------
        
        public override string RepositoryID => "core.general";

        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private GeneralAudio m_Audio;

        // PROPERTIES: ----------------------------------------------------------------------------

        public GeneralAudio Audio => this.m_Audio;
        
        // EDITOR ENTER PLAYMODE: -----------------------------------------------------------------

        #if UNITY_EDITOR
        
        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode() => Instance = null;
        
        #endif
    }
}