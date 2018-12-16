using backend.persistence.ef;
using System.Collections.Generic;
using support.system;

namespace backend.config{
    /// <summary>
    /// Represents a "configuration" class for backend module
    /// </summary>
    public sealed class BackendConfiguration{
        /// <summary>
        /// Current entity framework context
        /// </summary>
        public static MyCContext entityFrameworkContext{get;set;}

        /// <summary>
        /// Dictionary holding all MyCContexts 
        /// </summary>
        public static Properties entityFrameworkContexts = new Properties();
    }
}