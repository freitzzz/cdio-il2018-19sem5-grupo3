package cdiomyc.core.mv.authentication.session;

import cdiomyc.core.application.Application;
import cdiomyc.core.domain.auth.Session;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.support.encryptions.DigestUtils;
import java.time.format.DateTimeFormatter;

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
        authenticationSessionDetailsMV.sessionEnd=authenticationSession.getSessionEndDateTime().format(DateTimeFormatter.ISO_DATE_TIME);
        return authenticationSessionDetailsMV;
    }
    
    /**
     * Creates an authentication session secrete identifier
     * @param authenticationDetails AuthenticationMV with the authentication details
     * @return String with the authentication session secrete identifier
     */
    public static String createSecreteIdentifier(AuthenticationMV authenticationDetails){
        if(authenticationDetails.userAgent==null||authenticationDetails.secreteKey==null)
            throw new IllegalArgumentException("Invalid authentication secrete details");
        return DigestUtils.hashify(
                authenticationDetails.userAgent.concat(authenticationDetails.secreteKey),
                Application.settings().getAuthenticationSessionSecreteIdentifierDigestAlgorithm(),
                Application.settings().getAuthenticationSessionSecreteIdentifierDigestSalt().getBytes()
        );
    }
    
}
