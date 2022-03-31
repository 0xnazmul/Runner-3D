using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCreator.Runtime.Common
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private UnityEngine.Object m_Scene;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public string Name => this.m_Scene != null 
            ? this.m_Scene.name 
            : string.Empty;

        public int Index => GetSceneIndex(this.Name);
        
        // OPERATORS: -----------------------------------------------------------------------------

        public static implicit operator string(SceneReference sceneReference)
        {
            return sceneReference.Name;
        }
        
        public override string ToString()
        {
            return string.IsNullOrEmpty(this.Name) ? "(none)" : this.Name;
        }
        
        // STATIC METHODS: ------------------------------------------------------------------------

        public static int GetSceneIndex(string target)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);

                if (target == scenePath || sceneName == target) return i;
            }

            return -1;
        }
    }
}