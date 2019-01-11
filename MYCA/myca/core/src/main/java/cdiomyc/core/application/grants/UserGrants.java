package cdiomyc.core.application.grants;

import cdiomyc.core.domain.Role;
import cdiomyc.core.domain.User;

/**
 * Service for granting certain user operations
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class UserGrants {
    
    /**
     * Grants that a user is a client
     * @param user User with the user being granted
     */
    public static void grantUserIsClient(User user){
        grantUserIsValid(user);
        if(!user.hasRole(Role.CLIENT))
            throw new IllegalStateException("User is not a client!");
    }
    
    /**
     * Grants that a user is a content manager
     * @param user User with the user being granted
     */
    public static void grantUserIsContentManager(User user){
        grantUserIsValid(user);
        if(!user.hasRole(Role.CONTENT_MANAGER))
            throw new IllegalStateException("User is not a content manager!");
    }
    
    /**
     * Grants that a user is valid
     * @param user User with the user being granted
     */
    private static void grantUserIsValid(User user){
        if(user==null)
            throw new IllegalStateException("User is not valid!");
    }
}
