package cdiomyc.core.domain.auth;

import cdiomyc.core.domain.auth.credentials.CredentialsAuth;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;

/**
 * AuthFactory class
 *
 * @author João
 */
public class AuthFactory {

    /**
     * Creates an instance of one of Auth's subclasses
     *
     * @param authDetails MV containing the authentication details
     * @return instance of Auth
     */
    public static Auth createAuth(AuthenticationMV authDetails) {
        if (authDetails instanceof CredentialsAuthenticationMV) {
            CredentialsAuthenticationMV credentialDetails = (CredentialsAuthenticationMV) authDetails;
            return new CredentialsAuth(credentialDetails.username, credentialDetails.password);
        }
        throw new IllegalArgumentException("Invalid authentication details!");
    }
}
