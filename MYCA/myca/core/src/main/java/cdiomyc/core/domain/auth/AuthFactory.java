package cdiomyc.core.domain.auth;

import cdiomyc.core.domain.auth.credentials.CredentialsAuth;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;
import cdiomyc.core.mv.users.CreateCredentialsUserMV;
import cdiomyc.core.mv.users.CreateUserMV;

/**
 *
 * @author João
 */
public class AuthFactory {
    public static Auth createAuth(AuthenticationMV authDetails){
        if(authDetails instanceof CredentialsAuthenticationMV){
            CredentialsAuthenticationMV credentialDetails = (CredentialsAuthenticationMV)authDetails;
            return new CredentialsAuth(credentialDetails.username,credentialDetails.password);
        }
        throw new IllegalArgumentException("Invalid authentication details!");
    }
}
