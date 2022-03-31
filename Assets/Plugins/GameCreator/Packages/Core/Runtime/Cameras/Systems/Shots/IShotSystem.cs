namespace GameCreator.Runtime.Cameras
{
    public interface IShotSystem
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        bool Active { get; set; }
        
        // METHODS: -------------------------------------------------------------------------------
        
        void OnAwake(TShotType shotType);
        void OnStart(TShotType shotType);
        void OnDestroy(TShotType shotType);

        void OnUpdate(TShotType shotType);

        void OnEnable(TShotType shotType, TCamera camera);
        void OnDisable(TShotType shotType, TCamera camera);

        void OnDrawGizmos(TShotType shotType);
        void OnDrawGizmosSelected(TShotType shotType);
    }
}