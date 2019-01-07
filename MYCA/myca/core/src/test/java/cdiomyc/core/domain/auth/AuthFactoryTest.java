package cdiomyc.core.domain.auth;

import cdiomyc.core.domain.auth.credentials.UsernameCredentialsAuth;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * Test class of AuthFactory class
 *
 * @author João
 */
public class AuthFactoryTest {

    /**
     * Ensures createAuth creates proper instances of UsernameCredentialsAuth
     */
    @Test
    public void ensureCreateAuthCreatesCredentialAuthenticationMV() {
        CredentialsAuthenticationMV authDetails = new CredentialsAuthenticationMV();
        authDetails.username = "user1";
        authDetails.password = "user1";
        Auth expResult = new UsernameCredentialsAuth(authDetails.username, authDetails.password);
        Auth result = AuthFactory.createAuth(authDetails);
        assertEquals(expResult, result);
    }

    /**
     * Ensures createAuth fails if argument is null
     */
    @Test(expected = IllegalStateException.class)
    public void ensureCreateAuthFailsIfArgumentNull() {
        AuthFactory.createAuth(null);
    }

    /**
     * Ensures validateAuth fails if argument is not valid
     */
    @Test(expected = IllegalStateException.class)
    public void ensureValidateAuthFails() {
        CredentialsAuthenticationMV authDetails = new CredentialsAuthenticationMV();
        authDetails.username = "user1";
        authDetails.password = "user"; //this password is not lenghty enough so Password constructor will throw an IllegalArgumentException for validateAuth to catch
        AuthFactory.validateAuth(authDetails);
    }

}
