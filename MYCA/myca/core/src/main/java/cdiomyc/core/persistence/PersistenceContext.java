package cdiomyc.core.persistence;

import cdiomyc.core.application.Application;

/**
 * Persistence Utility class that represents the factory of persistence repositories 
 * factories for a certain persistence context
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class PersistenceContext {
    /**
     * Constant that represents the message that occurres if the current 
     * persistence context is not defined
     */
    private static final String PERSISTENCE_CONTEXT_NOT_DEFINED_MESSAGE
            ="The persistence context is not defined!";
    /**
     * RepositoryFactory with the current persistence context repository factory
     */
    private static RepositoryFactory contextRepositoryFactory;
    /**
     * Method that returns the repositories of the current persistence context
     * @return RepositoryFactory with the factory that allows the repositories 
     * retrieval of the current persistence context
     */
    public static RepositoryFactory repositories(){
        if(contextRepositoryFactory==null){
            try {
                return (RepositoryFactory)
                        Class.forName(Application.settings().getPersistenceContext()).newInstance();
            } catch (ClassNotFoundException | InstantiationException | IllegalAccessException persistenceContextException) {
                throw new IllegalStateException(PERSISTENCE_CONTEXT_NOT_DEFINED_MESSAGE,persistenceContextException);
            }
        }
        return contextRepositoryFactory;
    }
    /**
     * Hides default constructor
     */
    private PersistenceContext(){}
}
