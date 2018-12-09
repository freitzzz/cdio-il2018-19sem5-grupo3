package cdiomyc.core.domain.auth;

import cdiomyc.core.domain.auth.credentials.CredentialsAuth;
import cdiomyc.support.utils.JWTUtils;
import java.time.LocalDateTime;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * Test class of Session
 *
 * @author João
 */
public class SessionTest {

    /**
     * Ensure Session can not be created with invalid date
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureSessionCantBeCreatedWithInvalidDate() {
        new Session(LocalDateTime.now().minusDays(1), "token","secretetoken");
    }

    /**
     * Ensure Session can not be created with null date
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureSessionCantBeCreatedWithNullDate() {
        new Session(null, "token","secretetoken");
    }

    /**
     * Ensure Session can not be created with null token
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureSessionCantBeCreatedWithNullToken() {
        new Session(LocalDateTime.now().plusMinutes(60), null,"secretetoken");
    }

    /**
     * Ensure Session can not be created with invalid token
     */
    @Test(expected = IllegalArgumentException.class)
    public void ensureSessionCantBeCreatedWithInvalidToken() {
        new Session(LocalDateTime.now().plusMinutes(60), "","secretetoken");
    }

    /**
     * Ensure isActive works properly
     */
    @Test
    public void ensureIsActiveSucceeds() {
        Session instance = new Session(LocalDateTime.now().plusMinutes(60), "token","secretetoken");
        assertTrue(instance.isActive());
    }

    /**
     * Ensure tokenAsJWT works properly
     */
    @Test
    public void ensureTokenAsJWTSucceeds() {
        String token = "Sarastro";
        Session instance = new Session(LocalDateTime.now().plusMinutes(60), token,"secretetoken");
        assertEquals(instance.id(), JWTUtils.decode(instance.tokenAsJWT()));
    }

    /**
     * Ensures getSessionEndDateTime works properly
     */
    @Test
    public void ensureGetSessionEndDateTimeSucceeds() {
        String token = "Sarastro";
        LocalDateTime time = LocalDateTime.now().plusMinutes(60);
        Session instance = new Session(time, token,"secretetoken");
        assertEquals(time, instance.getSessionEndDateTime());
    }

    /**
     * Ensure hashCode works properly
     */
    @Test
    public void ensureHashCodeWorks() {
        String token = "Sarastro";
        Session instance = new Session(LocalDateTime.now().plusMinutes(60), token,"secretetoken");
        assertEquals(instance.id().hashCode(), instance.hashCode());
    }

    /**
     * Ensure equals method fails if argument is null
     */
    @Test
    public void ensureEqualsFailsIfNullArgument() {
        String token = "Sarastro";
        Session instance = new Session(LocalDateTime.now().plusMinutes(60), token,"secretetoken");
        assertFalse(instance.equals(null));
    }

    /**
     * Ensure equals method succeeds if instances are the same
     */
    @Test
    public void ensureEqualsSucceedsIfSameInstance() {
        String token = "Sarastro";
        Session instance = new Session(LocalDateTime.now().plusMinutes(60), token,"secretetoken");
        assertTrue(instance.equals(instance));
    }

    /**
     * Ensure equals method fails if instances are of different classes
     */
    @Test
    public void ensureEqualsFailsIfDifferentClass() {
        String token = "Sarastro";
        Session instance = new Session(LocalDateTime.now().plusMinutes(60), token,"secretetoken");
        assertFalse(instance.equals(new CredentialsAuth("username", "password")));
    }

    /**
     * Ensure equals method fails if instances are not equal
     */
    @Test
    public void ensureEqualsFailsIfInstancesNotEqual() {
        String token = "Sarastro";
        String token2 = "Magic Flute";
        Session instance = new Session(LocalDateTime.now().plusMinutes(60), token,"secretetoken");
        Session instance2 = new Session(LocalDateTime.now().plusMinutes(60), token2,"secretetoken");
        assertFalse(instance.equals(instance2));
    }
}
