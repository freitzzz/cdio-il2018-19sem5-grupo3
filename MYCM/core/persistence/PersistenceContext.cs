using System;

namespace core.persistence{
    /// <summary>
    /// Class that acts as a factory of factories for a certain persistence context
    /// </summary>
    public sealed class PersistenceContext{
        /// <summary>
        /// RepositoryFactory with the current persistence context repository factory
        /// </summary>
        private static RepositoryFactory contextRepositoryFactory;

        /// <summary>
        /// Returns all repositories available for the current persistence context
        /// </summary>
        /// <returns>RepositoryFactory with the available repositories</returns>
        public static RepositoryFactory repositories(){
            if(contextRepositoryFactory==null){
                RepositoryFactory repositoryFactory
                                        =(RepositoryFactory)Activator
                                                            .CreateInstance(Type
                                                                .GetType(Application
                                                                    .settings()
                                                                    .getPersistenceContext()));
                if(repositoryFactory==null)
                    throw new InvalidOperationException();
                contextRepositoryFactory=repositoryFactory;
            }
            return contextRepositoryFactory;
        }

        /// <summary>
        /// Hides default constructor
        /// </summary>
        private PersistenceContext(){}
    }
}