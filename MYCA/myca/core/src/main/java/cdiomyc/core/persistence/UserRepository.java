package cdiomyc.core.persistence;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.support.domain.ddd.Repository;

/**
 * Represents the repository functionalities for users
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public interface UserRepository extends Repository<User,Long,Auth>{
    /**
     * Finds an user by its authentication details
     * @param authenticationDetails AuthenticationMV with the authentication details
     * @return User with the user who has a certain authentication details
     */
    User findUserByAuthenticationDetails(AuthenticationMV authenticationDetails);
    
    /**
     * Finds an user by its session API token
     * @param sessionAPIToken String with the user API token
     * @return User with the user who has a certain session API token
     */
    User findUserBySessionAPIToken(String sessionAPIToken);
}
