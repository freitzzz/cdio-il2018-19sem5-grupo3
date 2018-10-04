using backend.persistence.ef;

namespace backend.config{
    /// <summary>
    /// Represents a "configuration" class for backend module
    /// </summary>
    public sealed class BackendConfiguration{
        /// <summary>
        /// Current entity framework context
        /// </summary>
        public static MyCContext entityFrameworkContext{get;set;}
    }
}