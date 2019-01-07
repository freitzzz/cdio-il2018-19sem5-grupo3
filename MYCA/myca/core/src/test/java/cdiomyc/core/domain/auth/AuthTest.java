package cdiomyc.core.domain.auth;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.credentials.UsernameCredentialsAuth;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * Test class of Auth
 * @author João
 */
public class AuthTest {
    /**
     * Ensure id method works
     */
    @Test
    public void ensureIdWorks() {
        Auth instance = new UsernameCredentialsAuth("username", "password");
        Auth instance2 = new UsernameCredentialsAuth("username", "password");
        assertEquals(instance2.id(), instance.id());
    }

    /**
     * Ensure hashCode method works
     */
    @Test
    public void ensureHashCodeWorks() {
        Auth instance = new UsernameCredentialsAuth("username", "password");
        assertEquals(instance.id().hashCode(), instance.hashCode());
    }

    /**
     * Ensures equals returns true if argument is the same instance
     */
    @Test
    public void ensureEqualsReturnsTrueIfSameInstance() {
        Auth instance = new UsernameCredentialsAuth("username", "password");
        assertTrue(instance.equals(instance));
    }

    /**
     * Ensures equals returns false if argument is null
     */
    @Test
    public void ensureEqualsReturnsFalseIfArgumentIsNull() {
        Auth instance = new UsernameCredentialsAuth("username", "password");
        Auth instance2 = null;
        assertFalse(instance.equals(instance2));
    }

    /**
     * Ensures equals returns false if argument is not of the same class
     */
    @Test
    public void ensureEqualsReturnsFalseIfArgumentNotSameClass() {
        Auth instance = new UsernameCredentialsAuth("username", "password");
        User instance2 = new User(instance);
        assertFalse(instance.equals(instance2));
    }

    /**
     * Ensures equals returns false if argument is not equal
     */
    @Test
    public void ensureEqualsReturnsFalseIfArgumentNotEqual() {
        Auth instance = new UsernameCredentialsAuth("username", "password");
        Auth instance2 = new UsernameCredentialsAuth("username1", "password1");
        assertFalse(instance.equals(instance2));
    }
}
