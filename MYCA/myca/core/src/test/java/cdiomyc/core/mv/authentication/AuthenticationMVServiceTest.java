package cdiomyc.core.mv.authentication;

import org.junit.Test;
import static org.junit.Assert.*;

/**
 * Test class of AuthenticationMVService
 *
 * @author João
 */
public class AuthenticationMVServiceTest {

    /**
     * Ensure classFromType returns CredentialsAuthenticationMV class
     */
    @Test
    public void ensureClassFromTypeReturnsCredentialsAuthenticationMV() {
        String type = "credentials";
        Class expResult = CredentialsAuthenticationMV.class;
        Class result = AuthenticationMVService.classFromType(type);
        assertEquals(expResult, result);
    }

    /**
     * Ensure classFromType fails if null argument
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureClassFromTypeFailsWIthNullArgument() {
        String type = null;
        AuthenticationMVService.classFromType(type);
    }

    /**
     * Ensure classFromType fails if empty argument
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureClassFromTypeFailsWIthEmptyArgument() {
        String type = "";
        AuthenticationMVService.classFromType(type);
    }

    /**
     * Ensure classFromType fails if invalid argument
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureClassFromTypeFailsWIthInvalidArgument() {
        String type = "Puddle of Tearful Rain";
        AuthenticationMVService.classFromType(type);
    }

}
