package cdiomyc.core.application.auth;

import cdiomyc.core.application.grants.CRUDGrants;
import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.AuthFactory;
import cdiomyc.core.domain.auth.Session;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.session.AuthenticationSessionMVService;
import cdiomyc.core.mv.authentication.session.GetAuthenticationSessionDetailsMV;
import cdiomyc.core.persistence.PersistenceContext;

/**
 * Application controller that serves authentication operations
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class AuthenticationController {
    /**
     * Authenticates an user into all MYCA APIs
     * @param authenticationModelView AuthenticationMV with the required authentication details
     * @return GetAuthenticationSessionDetailsMV with the authentication details model view
     */
    public static GetAuthenticationSessionDetailsMV authenticate(AuthenticationMV authenticationModelView){
        User userToAuthenticate
                =PersistenceContext.repositories().createUserRepository()
                        .findUserByAuthToken(AuthFactory.validateAuth(authenticationModelView).id());
        CRUDGrants.grantFindWasSuccessful(userToAuthenticate);
        Session createdUserSession=userToAuthenticate.createNewSession(AuthenticationSessionMVService.createSecreteIdentifier(authenticationModelView));
        User authenticatedUser
                =PersistenceContext.repositories().createUserRepository().update(userToAuthenticate);
        CRUDGrants.grantUpdateWasSuccessful(authenticatedUser);
        return AuthenticationSessionMVService.fromEntity(createdUserSession);
    }
}
