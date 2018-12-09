package cdiomyc.core.mv.authorization;

import cdiomyc.support.utils.JWTUtils;

/**
 * Service for creating and manipulating User Authorization Model Views
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class AuthorizationMVService {
    /**
     * Returns the user session API token
     * @param userAuthorizationMV IsUserAuthorizedMV with the user authorization details model view
     * @return String with the user session API token
     */
    public static String getSessionAPIToken(IsUserAuthorizedMV userAuthorizationMV){
        return JWTUtils.decode(userAuthorizationMV.sessionAPIToken);
    }
}
