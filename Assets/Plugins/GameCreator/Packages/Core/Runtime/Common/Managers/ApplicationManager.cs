using System;
using UnityEngine;

namespace GameCreator.Runtime.Common
{
    [AddComponentMenu("")]
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        // STATIC PROPERTIES: ---------------------------------------------------------------------

        public static bool IsExiting { get; private set; }
        
        // STATIC EVENTS: -------------------------------------------------------------------------

        public static event Action EventExit;

        // INITIALIZE METHODS: --------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void OnSubsystemsInit()
        {
            IsExiting = false;
            Instance.WakeUp();
        }
        
        private void OnApplicationQuit()
        {
            IsExiting = true;
            EventExit?.Invoke();
        }
    }
}