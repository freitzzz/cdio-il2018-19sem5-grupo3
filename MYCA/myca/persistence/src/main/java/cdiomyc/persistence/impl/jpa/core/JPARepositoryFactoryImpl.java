package cdiomyc.persistence.impl.jpa.core;

import cdiomyc.core.persistence.RepositoryFactory;
import cdiomyc.core.persistence.UserRepository;

/**
 * JPARepositoryFactoryImpl class, represents the JPA implementation of the
 repository factory
 *
 * @author João
 */
public class JPARepositoryFactoryImpl implements RepositoryFactory {
    
    /**
     * Creates an User repository
     *
     * @return instance of JPAUserRepositoryImpl
     */
    @Override
    public UserRepository createUserRepository() {
        return new JPAUserRepositoryImpl();
    }

}
