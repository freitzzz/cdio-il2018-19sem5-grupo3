package cdiomyc.persistence.impl.jpa.core;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;
import cdiomyc.core.persistence.UserRepository;
import cdiomyc.persistence.impl.jpa.BaseJPARepository;
import javax.persistence.Query;

/**
 * JPAUserRepositoryImpl class, represents the JPA implementation of the User
 * repository
 *
 * @author João
 */
public class JPAUserRepositoryImpl extends BaseJPARepository<User, Long> implements UserRepository {

    /**
     * Creates a new instance of JPAUserRepositoryImpl
     */
    public JPAUserRepositoryImpl() {
        super(PersistenceUnitName.VALUE);
    }

    /**
     * Finds a User by its identifier
     *
     * @param domainEntityIdentifier EID with the User identifier
     * @return User which is identified by a certain identifier
     */
    @Override
    public User findEID(Auth domainEntityIdentifier) {
        return findUserByAuthToken(domainEntityIdentifier.id());
    }

    /**
     * Finds User by Authentication details
     *
     * @param authenticationDetails
     * @return user corresponding to the authentication details
     * @throws IllegalArgumentException if no user was found
     */
    @Override
    public User findUserByAuthenticationDetails(AuthenticationMV authenticationDetails) {
        if (authenticationDetails instanceof CredentialsAuthenticationMV) {
            Query query = super.getEntityManager().createQuery("SELECT u FROM User u, CredentialsAuth ca "
                    + "WHERE u.auth = ca "
                    + "AND ca.username.value = :username "
                    + "AND ca.password.value = :password")
                    .setParameter("username", ((CredentialsAuthenticationMV) authenticationDetails).username)
                    .setParameter("password", ((CredentialsAuthenticationMV) authenticationDetails).password)
                    .setMaxResults(1);
            return (User) query.getSingleResult();
        }
        throw new IllegalArgumentException("No User found with the given authentication details!");
    }

    /**
     * Finds an user by its session API token
     *
     * @param sessionAPIToken String with the user API token
     * @return User with the user who has a certain session API token
     */
    @Override
    public User findUserBySessionAPIToken(String sessionAPIToken) {
        Query userBySessionAPITokenQuery = super.getEntityManager()
                .createQuery("SELECT U from User U,Session US "
                        + "WHERE US.sessionToken= :sessionAPIToken "
                        + "AND US MEMBER OF U.sessions")
                .setParameter("sessionAPIToken", sessionAPIToken)
                .setMaxResults(1);
        Object userBySessionAPI = !userBySessionAPITokenQuery.getResultList().isEmpty() ? userBySessionAPITokenQuery.getSingleResult() : null;
        if (userBySessionAPI == null) {
            throw new IllegalStateException("No user found with the given session API token");
        }
        return (User) userBySessionAPI;
    }

    /**
     * Finds an user by its auth token
     *
     * @param authToken String with the auth token
     * @return User with the user who is identified by the auth token
     */
    @Override
    public User findUserByAuthToken(String authToken) {
        Query userByAuthTokenQuery = super
                .getEntityManager()
                .createQuery("SELECT U FROM User U,Auth UA "
                        + "WHERE UA.token= :authToken "
                        + "AND U.auth=UA")
                .setParameter("authToken", authToken)
                .setMaxResults(1);
        Object userByAuthToken = !userByAuthTokenQuery.getResultList().isEmpty() ? userByAuthTokenQuery.getSingleResult() : null;
        if (userByAuthToken == null) {
            throw new IllegalStateException("No user found with the given auth token");
        }
        return (User) userByAuthToken;
    }
}
