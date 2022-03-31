namespace GameCreator.Runtime.Characters
{
    public interface IInteractive
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        int ID { get; }
        int Priority { get; }

        // METHODS: -------------------------------------------------------------------------------
        
        void Interact(int interactionID);
    }
}