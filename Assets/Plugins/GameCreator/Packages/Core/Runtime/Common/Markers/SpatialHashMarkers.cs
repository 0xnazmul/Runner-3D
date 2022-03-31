using UnityEngine;

namespace GameCreator.Runtime.Common
{
    public static class SpatialHashMarkers
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