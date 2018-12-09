package cdiomyc.core.persistence;

/**
 * Represents a factory of repositories
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public interface RepositoryFactory {
    /**
     * Creates a new UserRepository
     * @return UserRepository with the created user repository
     */
    UserRepository createUserRepository();
}
