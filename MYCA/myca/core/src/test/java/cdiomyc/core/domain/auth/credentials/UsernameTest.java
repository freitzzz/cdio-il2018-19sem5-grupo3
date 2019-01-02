package cdiomyc.core.domain.auth.credentials;

import org.junit.Test;
import static org.junit.Assert.*;

/**
 * Test class of Username
 *
 * @author João
 */
public class UsernameTest {

    @Test(expected = IllegalArgumentException.class)
    public void ensureUsernameCantBeCreatedWithNullArgument() {
        Username.valueOf(null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensureUsernameCantBeSmall() {
        Username.valueOf("ye");
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensureUsernameCantBeTooLengthy() {
        Username.valueOf("BITCONEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEECT");
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensureUsernameCantHaveSpecialCharacters() {
        Username.valueOf("Can i create this?");
    }

    @Test
    public void ensureEqualsFailsWithNullArgument() {
        Username user1 = Username.valueOf("username1");
        assertFalse(user1.equals(null));
    }

    @Test
    public void ensureEqualsFailsIfNotSameClass() {
        Username user1 = Username.valueOf("username1");
        assertFalse(user1.equals("IT IS I"));
    }

    @Test
    public void ensureEqualsSucceedsIfSameInstance() {
        Username user1 = Username.valueOf("username1");
        assertTrue(user1.equals(user1));
    }

    @Test
    public void ensureEqualsFailsIfInstancesAreNotEqual() {
        Username user1 = Username.valueOf("username1");
        Username user2 = Username.valueOf("username2");
        assertFalse(user1.equals(user2));
    }

    @Test
    public void ensureEqualsSucceedsIfInstancesAreEqual() {
        Username user1 = Username.valueOf("username1");
        Username user2 = Username.valueOf("username1");
        assertTrue(user1.equals(user2));
    }

    @Test
    public void ensureHashCodeWorks() {
        Username user1 = Username.valueOf("username1");
        assertEquals(user1.toString().hashCode(), user1.hashCode());
    }
}
