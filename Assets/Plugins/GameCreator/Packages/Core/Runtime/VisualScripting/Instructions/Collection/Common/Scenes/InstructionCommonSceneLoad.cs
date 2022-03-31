using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCreator.Runtime.VisualScripting 
{
    [Version(0, 1, 1)]

    [Title("Load Scene")]
    [Description("Loads a new Scene")]

    [Category("Scenes/Load Scene")]

    [Parameter(
        "Scene",
        "The scene to be loaded"
    )]

    [Parameter(
        "Mode",
        "Single mode replaces all other scenes. Additive mode loads the scene on top of the others"
    )]
    
    [Parameter(
        "Async",
        "Loads the scene in the background or freeze the game until its done"
    )]
    
    [Parameter(
        "Scene Entries",
        "Define the starting location of the player and other characters after loading the scene"
    )]

    [Keywords("Change")]
    [Image(typeof(IconUnity), ColorTheme.Type.TextNormal)]
    
    [Serializable]
    public class InstructionCommonSceneLoad : Instruction
    {
        [Serializable]
        public class SceneEntries
        {
            [Serializable]
            private class EntryData
            {
                public PropertyGetGameObject m_Character = GetGameObjectPlayer.Create();
                public PropertyGetLocation m_Location = GetLocationNone.Create;
            }
            
            // MEMBERS: ---------------------------------------------------------------------------
        
            [SerializeField] private List<EntryData> m_EntryPoints = new List<EntryData>();

            // PUBLIC METHODS: --------------------------------------------------------------------
            
            public void Schedule(int scene, Args args)
            {
                foreach (EntryData entryData in this.m_EntryPoints)
                {
                    RoomManager.Instance.Subscribe(scene, () =>
                    {
                        Character character = entryData.m_Character.Get<Character>(args);
                        Location location = entryData.m_Location.Get(args);
                        if (character == null) return;
                        
                        if (location.HasPosition) character.Driver.SetPosition(location.Position);
                        if (location.HasRotation) character.Driver.SetRotation(location.Rotation);
                    });
                }
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////////////////////
        // EXPOSED MEMBERS: -----------------------------------------------------------------------
        
        [SerializeField] private PropertyGetScene m_Scene = new PropertyGetScene();
        
        [SerializeField] private LoadSceneMode m_Mode = LoadSceneMode.Single;
        [SerializeField] private bool m_Async = false;
        
        [SerializeField] private SceneEntries m_SceneEntries = new SceneEntries();

        // MEMBERS: -------------------------------------------------------------------------------
        
        private AsyncOperation m_AsyncOperation;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => string.Format(
            "Load{0} scene {1}{2}",
            this.m_Mode == LoadSceneMode.Additive ? " additive" : string.Empty,
            this.m_Scene,
            this.m_Async ? " (async)" : string.Empty
        );

        // RUN METHOD: ----------------------------------------------------------------------------
        
        protected override async Task Run(Args args)
        {
            int scene = this.m_Scene.Get(args);
            this.m_SceneEntries.Schedule(scene, args);
            
            if (this.m_Async)
            {
                this.m_AsyncOperation = SceneManager.LoadSceneAsync(scene, this.m_Mode);
                await this.Until(() => this.m_AsyncOperation.isDone);
            }
            else
            {
                SceneManager.LoadScene(scene, this.m_Mode);
            }
        }
    }
}