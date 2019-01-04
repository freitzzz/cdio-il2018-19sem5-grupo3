package cdiomyc.core.application.autho;

import cdiomyc.core.application.grants.SessionGrants;
import cdiomyc.core.application.grants.UserGrants;
import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Session;
import cdiomyc.core.mv.authorization.AuthorizationMVService;
import cdiomyc.core.mv.authorization.IsUserAuthorizedMV;
import cdiomyc.core.persistence.PersistenceContext;
import cdiomyc.core.persistence.UserRepository;

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
        UserRepository userRepository=PersistenceContext
                .repositories()
                .createUserRepository();
        User userBeingAuthorized=userRepository
                .findUserBySessionAPIToken(userSessionAPIToken);
        UserGrants.grantUserIsClient(userBeingAuthorized);
        if(userAuthorizationDetails.isAdministrator)UserGrants.grantUserIsAdministrator(userBeingAuthorized);
        if(userAuthorizationDetails.isContentManager)UserGrants.grantUserIsContentManager(userBeingAuthorized);
        if(userAuthorizationDetails.isLogisticManager)UserGrants.grantUserIsLogisticManager(userBeingAuthorized);
        Session userLastSession=userBeingAuthorized.getLastSession();
        SessionGrants.grantSessionHasToken(userLastSession,userSessionAPIToken);
        SessionGrants.grantSessionIsActive(userLastSession);
        try{
            SessionGrants.grantSessionSecreteIdentifierIsTheSame(userLastSession,userAuthorizationDetails);
        }catch(IllegalStateException invalidSecreteIdentifier){
            userBeingAuthorized.endSession();
            userRepository.update(userBeingAuthorized);
            throw invalidSecreteIdentifier;
        }
    }
}
