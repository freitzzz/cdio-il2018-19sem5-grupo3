package cdiomyc.core.application.autho;

import cdiomyc.core.application.grants.SessionGrants;
import cdiomyc.core.application.grants.UserGrants;
import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Session;
import cdiomyc.core.mv.authorization.AuthorizationMVService;
import cdiomyc.core.mv.authorization.IsUserAuthorizedMV;
import cdiomyc.core.persistence.PersistenceContext;

/**
 * Application controller that serves authorization operations
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class AuthorizationController {
    /**
     * Checks if a user is authorized to perform an action
     * @param userAuthorizationDetails IsUserAuthorizedMV with the user authorization action details
     */
    public static void isAuthorized(IsUserAuthorizedMV userAuthorizationDetails){
        String userSessionAPIToken=AuthorizationMVService.getSessionAPIToken(userAuthorizationDetails);
        User userBeingAuthorized=PersistenceContext
                .repositories()
                .createUserRepository()
                .findUserBySessionAPIToken(userSessionAPIToken);
        UserGrants.grantUserIsClient(userBeingAuthorized);
        if(userAuthorizationDetails.isContentManager)UserGrants.grantUserIsContentManager(userBeingAuthorized);
        Session userLastSession=userBeingAuthorized.getLastSession();
        SessionGrants.grantSessionHasToken(userLastSession,userSessionAPIToken);
        SessionGrants.grantSessionIsActive(userLastSession);
    }
}
