using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    public class EventSystemManager : Singleton<EventSystemManager>
    {
        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitialize()
        {
            Instance.WakeUp();
        }
        
        // INITIALIZER: ---------------------------------------------------------------------------
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            SceneManager.sceneLoaded += this.OnSceneLoad;
            this.Initialize();
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            this.Initialize();
        }
        
        // PUBLIC METHODS: ------------------------------------------------------------------------

        public static bool RequestEventSystem()
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            BaseInputModule module = FindObjectOfType<BaseInputModule>();

            if (eventSystem == null)
            {
                Debug.LogError("<b>Event System:</b> No instance found");
                return false;
            }

            if (module == null)
            {
                Debug.LogError("<b>Event System:</b> No module found");
                return false;
            }

            return true;
        }
        
        // PRIVATE METHODS: -----------------------------------------------------------------------

        private void Initialize()
        {
            GameObject main = ShortcutMainCamera.Instance;
            if (main == null) return;
            
            PhysicsRaycaster raycaster3D = main.Get<PhysicsRaycaster>();
            Physics2DRaycaster raycaster2D = main.Get<Physics2DRaycaster>();
                
            if (raycaster3D == null) main.gameObject.Add<PhysicsRaycaster>();
            if (raycaster2D == null) main.gameObject.Add<Physics2DRaycaster>();
        }
    }
}
