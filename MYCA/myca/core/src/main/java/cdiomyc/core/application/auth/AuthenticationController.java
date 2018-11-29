package cdiomyc.core.application.auth;

import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.GetAuthenticationDetailsMV;

/**
 * Application controller that serves authentication operations
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class AuthenticationController {
    /**
     * Authenticates an user into all MYCA APIs
     * @param authenticationModelView AuthenticationMV with the required authentication details
     * @return GetAuthenticationDetailsMV with the authentication details model view
     */
    public static GetAuthenticationDetailsMV authenticate(AuthenticationMV authenticationModelView){
        throw new UnsupportedOperationException("#TODO:Implement!!!");
    }
}
