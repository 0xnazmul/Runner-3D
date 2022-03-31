using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCreator.Runtime.Common.SaveSystem
{
    public class Scenes : IGameSave
    {
        [Serializable]
        public class Token
        {
            // MEMBERS: ---------------------------------------------------------------------------
            
            [SerializeField] private string[] m_Names;

            // PROPERTIES: ------------------------------------------------------------------------

            public int Count => this.m_Names.Length;
            public string[] Names => this.m_Names;

            // CONSTRUCTOR: -----------------------------------------------------------------------

            private Token()
            {
                int scenesLength = SceneManager.sceneCount;
                this.m_Names = new string[scenesLength];

                for (int i = 0; i < scenesLength; ++i)
                {
                    this.m_Names[i] = SceneManager.GetSceneAt(i).name;
                }
            }

            public static Token Create() => new Token();
        }

        // IGAMESAVE: -----------------------------------------------------------------------------

        public string SaveID => "scenes";
        public bool IsShared => false;
        
        public Type SaveType => typeof(Token);
        public object SaveData => Token.Create();

        public LoadMode LoadMode => LoadMode.Greedy;

        public async Task OnLoad(object value)
        {
            Token token = value as Token;
            if (token == null) throw new Exception("Cannot convert 'token' to 'Scenes.Token'");

            if (token.Count == 0) return;

            await this.LoadScene(
                token.Names[0],
                LoadSceneMode.Single
            );

            for (int i = 1; i < token.Count; ++i)
            {
                await this.LoadScene(
                    token.Names[i],
                    LoadSceneMode.Additive
                );
            }
            
            await Task.Yield();
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private async Task LoadScene(string name, LoadSceneMode mode)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(name, mode);
            while (!AsyncManager.ExitRequest && !async.isDone) await Task.Yield();
        }
    }
}