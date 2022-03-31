namespace GameCreator.Runtime.Common
{
    public static class ExecutionOrderUtils
    {
        /// <summary>
        /// Use to setup connections between objects that do not require other dependencies.
        /// </summary>
        public const int EARLY   = -100;
        
        /// <summary>
        /// The default execution time. Should not be used.
        /// </summary>
        public const int DEFAULT = 0;
        
        /// <summary>
        /// Use when dependencies can't be set up earlier, so instead, delay the execution order
        /// of the dependencies.
        /// </summary>
        public const int LATER   = 100;
    }
}
