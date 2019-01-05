package cdiomyc.core.mv.users;

import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.domain.auth.credentials.UsernameCredentialsAuth;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author João
 */
public class UserMVServiceTest {

    /**
     * Ensures classFromType returns CreateCredentialsUserMV class
     */
    @Test
    public void ensureClassFromTypeReturnsCreateCredentialsUserMV() {
        String type = CredentialsAuthenticationMV.TYPE;
        Class expResult = CreateCredentialsUserMV.class;
        Class result = UserMVService.classFromType(type);
        assertEquals(expResult, result);
    }

    /**
     * Ensures classFromType fails if argument is null
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureClassFromTypeFailsIfArgumentIsNull() {
        UserMVService.classFromType(null);
    }

    /**
     * Ensures classFromType fails if argument is empty
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureClassFromTypeFailsIfArgumentIsEmpty() {
        UserMVService.classFromType("");
    }

    /**
     * Ensures classFromType fails if argument is not valid
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureClassFromTypeFailsIfArgumentIsNotValid() {
        UserMVService.classFromType("kurisutina");
    }

    /**
     * Ensure createdUserFromAuth works properly
     */
    public void ensureCreatedUserFromMVSucceeds() {
        Auth auth = new UsernameCredentialsAuth("username", "password");
        CreatedUserMV created = UserMVService.createdUserMVFromAuth(auth);
        assertEquals(auth.id(), created.token);
    }

}
