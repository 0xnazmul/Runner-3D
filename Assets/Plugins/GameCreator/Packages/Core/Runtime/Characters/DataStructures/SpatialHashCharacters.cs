using GameCreator.Runtime.Common;
using UnityEngine;

namespace GameCreator.Runtime.Characters
{
    public static class SpatialHashCharacters
    {
        // PROPERTIES: ----------------------------------------------------------------------------

        public static SpatialHash Get { get; private set; } = new SpatialHash();
        
        // INIT METHODS: --------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemsInit()
        {
            Get = new SpatialHash();
        }
    }
}