package cdiomyc.core.domain.auth;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.credentials.CredentialsAuth;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author João
 */
public class AuthTest {

    public AuthTest() {
    }

    @BeforeClass
    public static void setUpClass() {
    }

    @AfterClass
    public static void tearDownClass() {
    }

    @Before
    public void setUp() {
    }

    @After
    public void tearDown() {
    }

    /**
     * Ensure id method works
     */
    @Test
    public void ensureIdWorks() {
        Auth instance = new CredentialsAuth("username", "password");
        Auth instance2 = new CredentialsAuth("username", "password");
        assertEquals(instance2.id(), instance.id());
    }

    /**
     * Ensure hashCode method works
     */
    @Test
    public void ensureHashCodeWorks() {
        Auth instance = new CredentialsAuth("username", "password");
        assertEquals(instance.id().hashCode(), instance.hashCode());
    }

    /**
     * Ensures equals returns true if argument is the same instance
     */
    @Test
    public void ensureEqualsReturnsTrueIfSameInstance() {
        Auth instance = new CredentialsAuth("username", "password");
        assertTrue(instance.equals(instance));
    }

    /**
     * Ensures equals returns false if argument is null
     */
    @Test
    public void ensureEqualsReturnsFalseIfArgumentIsNull() {
        Auth instance = new CredentialsAuth("username", "password");
        Auth instance2 = null;
        assertFalse(instance.equals(instance2));
    }

    /**
     * Ensures equals returns false if argument is not of the same class
     */
    @Test
    public void ensureEqualsReturnsFalseIfArgumentNotSameClass() {
        Auth instance = new CredentialsAuth("username", "password");
        User instance2 = new User(instance);
        assertFalse(instance.equals(instance2));
    }

    /**
     * Ensures equals returns false if argument is not equal
     */
    @Test
    public void ensureEqualsReturnsFalseIfArgumentNotEqual() {
        Auth instance = new CredentialsAuth("username", "password");
        Auth instance2 = new CredentialsAuth("username1", "password1");
        assertFalse(instance.equals(instance2));
    }
}
