package cdiomyc.core.domain.auth.credentials;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertFalse;
import static org.junit.Assert.assertTrue;
import org.junit.Test;

/**
 * Test class of Password
 *
 * @author João
 */
public class PasswordTest {

    @Test(expected = IllegalArgumentException.class)
    public void ensurePasswordCantBeCreatedWithNullArgument() {
        Password.valueOf(null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensurePasswordCantBeTooSmall() {
        Password.valueOf("yey");
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensurePasswordCantBeTooBig() {
        Password.valueOf("BITCONEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEECT");
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensurePasswordCantHaveSpecialCharacters() {
        Password.valueOf("yey??");
    }

    @Test
    public void ensureEqualsFailsWithNullArgument() {
        Password password = Password.valueOf("password1");
        assertFalse(password.equals(null));
    }

    @Test
    public void ensureEqualsFailsIfNotSameClass() {
        Password password = Password.valueOf("password1");
        assertFalse(password.equals("IT IS I"));
    }

    @Test
    public void ensureEqualsSucceedsIfSameInstance() {
        Password password = Password.valueOf("password1");
        assertTrue(password.equals(password));
    }

    @Test
    public void ensureEqualsFailsIfInstancesAreNotEqual() {
        Password password1 = Password.valueOf("password1");
        Password password2 = Password.valueOf("password2");
        assertFalse(password1.equals(password2));
    }

    @Test
    public void ensureEqualsSucceedsIfInstancesAreEqual() {
        Password password1 = Password.valueOf("password1");
        Password password2 = Password.valueOf("password1");
        assertTrue(password1.equals(password2));
    }

    @Test
    public void ensureHashCodeWorks() {
        Password password1 = Password.valueOf("password1");
        assertEquals(password1.toString().hashCode(), password1.hashCode());
    }
}
