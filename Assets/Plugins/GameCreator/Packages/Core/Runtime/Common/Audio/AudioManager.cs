using System.Threading.Tasks;
using GameCreator.Runtime.Common.Audio;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    public class AudioManager : Singleton<AudioManager>, IGameSave
    {
        // PRIVATE PROPERTIES: --------------------------------------------------------------------

        protected override bool SurviveSceneLoads => true;
        
        // PUBLIC PROPERTIES: ---------------------------------------------------------------------

        /// <summary>
        /// Control the master volume or each audio system separately
        /// </summary>
        public Volume Volume { get; private set; } = new Volume();

        /// <summary>
        /// Sound effects that are played only once, like gun shots, footsteps, slashes
        /// or tension stings. 
        /// </summary>
        public SoundEffect SoundEffect { get; private set; }
        
        /// <summary>
        /// Music and ambient sounds that are looped and will persist until they are
        /// instructed to fade out.
        /// </summary>
        public Ambient Ambient { get; private set; }
        
        /// <summary>
        /// Sound effects that are played once by a character, such as a voiced dialogue
        /// line or a grunt. very similar to Sound Effects, but specifically designed for
        /// characters.
        /// </summary>
        public Speech Speech { get; private set; }
        
        /// <summary>
        /// Beeps and sounds played by buttons and other non-diegetic UI elements.
        /// </summary>
        public UserInterface UserInterface { get; private set; }

        // CONSTRUCTOR: ---------------------------------------------------------------------------

        protected override void OnCreate()
        {
            base.OnCreate();

            Transform soundEffectsParent = this.CreateParent("Sound Effects");
            Transform ambientParent = this.CreateParent("Ambient");
            Transform voicesParent = this.CreateParent("Voices");
            Transform userInterfacesParent = this.CreateParent("User Interface");

            this.SoundEffect = new SoundEffect(soundEffectsParent.transform);
            this.Ambient = new Ambient(ambientParent.transform);
            this.Speech = new Speech(voicesParent.transform);
            this.UserInterface = new UserInterface(userInterfacesParent.transform);

            ApplicationManager.EventExit += this.OnExit;

            _ = SaveLoadManager.Subscribe(this);
        }

        // UPDATE METHOD: -------------------------------------------------------------------------

        private void Update()
        {
            this.Volume.Update();
            
            this.SoundEffect.Update();
            this.Ambient.Update();
            this.Speech.Update();
            this.UserInterface.Update();
        }

        // PRIVATE METHODS: -----------------------------------------------------------------------

        private async Task StopAll(float duration)
        {
            await Task.WhenAll(
                this.SoundEffect.StopAll(duration),
                this.Ambient.StopAll(duration),
                this.Speech.StopAll(duration),
                this.UserInterface.StopAll(duration)
            );
        }
        
        private Transform CreateParent(string id)
        {
            GameObject instance = new GameObject(id);
            instance.transform.SetParent(this.transform);

            return instance.transform;
        }

        private void OnExit()
        {
            this.SoundEffect.OnExit();
            this.Ambient.OnExit();
            this.Speech.OnExit();
            this.UserInterface.OnExit();
        }
        
        // IGAMESAVE: -----------------------------------------------------------------------------

        public string SaveID => "volumes";
        public bool IsShared => true;

        public System.Type SaveType => typeof(Volume);
        public object SaveData => this.Volume;
        
        public LoadMode LoadMode => LoadMode.Greedy;
        
        public Task OnLoad(object value)
        {
            _ = this.StopAll(0.5f);
            
            this.Volume = value as Volume ?? new Volume();
            return Task.FromResult(true);
        }
    }
}
