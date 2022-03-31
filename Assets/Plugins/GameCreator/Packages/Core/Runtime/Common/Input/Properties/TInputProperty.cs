namespace GameCreator.Runtime.Common
{
    public abstract class TInputProperty
    {
        // PROPERTIES: ---------------------------------------------------------------------------- 
        
        protected abstract TInput Input { get; }

        public bool IsEnabled => this.Input.Active;
        
        // INITIALIZERS: --------------------------------------------------------------------------
        
        public void OnStartup() => this.Input.OnStartup();
        public void OnDispose() => this.Input.OnDispose();
        public void OnUpdate()  => this.Input.OnUpdate();

        public void Enable()  => this.Input.Active = true;
        public void Disable() => this.Input.Active = false;
        
        // STRING: --------------------------------------------------------------------------------

        public abstract override string ToString();
    }
}