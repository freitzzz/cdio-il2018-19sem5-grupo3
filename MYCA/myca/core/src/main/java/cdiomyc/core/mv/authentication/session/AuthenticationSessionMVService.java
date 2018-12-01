package cdiomyc.core.mv.authentication.session;

import cdiomyc.core.domain.auth.Session;

/**
 * Service for creating and manipulating AuthenticationSession Model Views
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class AuthenticationSessionMVService {
    /**
     * Returns the details of an authentication session
     * @param authenticationSession Session with the authentication session
     * @return GetAuthenticationSessionDetailsMV with the authentication session details model view
     */
    public static GetAuthenticationSessionDetailsMV fromEntity(Session authenticationSession){
        GetAuthenticationSessionDetailsMV authenticationSessionDetailsMV=new GetAuthenticationSessionDetailsMV();
        authenticationSessionDetailsMV.token=authenticationSession.tokenAsJWT();
        authenticationSessionDetailsMV.sessionEnd=authenticationSession.getSessionEndDateTime();
        return authenticationSessionDetailsMV;
    }
}
