package cdiomyc.core.domain;

import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.domain.auth.Session;
import cdiomyc.core.domain.auth.credentials.UsernameCredentialsAuth;
import static org.junit.Assert.*;
import org.junit.Test;

/**
 *
 * @author João
 */
public class UserTest {

    @Test(expected = IllegalArgumentException.class)
    public void ensureUserCreationFailsIfNullArgument() {
        new User(null);
    }

    @Test
    public void ensureNewSessionIsCreatedIfUserDoesNotHaveActiveSession() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        Session session = user.createNewSession();
        assertTrue(user.getLastSession().equals(session));
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensureNewSessionIsNotCreatedIfUserHasActiveSession() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        user.createNewSession();
        user.createNewSession();
    }

    @Test
    public void ensureGetLastSessionWorks() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        Session session = user.createNewSession();
        assertTrue(user.getLastSession().equals(session));
    }

    @Test
    public void ensureGetLastSessionReturnsNullIfUserHasNoSessions() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        assertTrue(user.getLastSession() == null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensureAddRoleFailsIfRoleIsNull() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        user.addRole(null);
    }

    @Test(expected = IllegalStateException.class)
    public void ensureAddRoleFailsIfUserHasRole() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        user.addRole(Role.CLIENT);
    }

    @Test
    public void ensureAddRoleSucceeds() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        user.addRole(Role.CONTENT_MANAGER);
        assertTrue(user.hasRole(Role.CONTENT_MANAGER));
    }

    @Test(expected = IllegalArgumentException.class)
    public void ensureRemoveRoleFailsIfRoleIsNull() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        user.removeRole(null);
    }

    @Test(expected = IllegalStateException.class)
    public void ensureRemoveRoleFailsIfUserDoesNotHaveRole() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        user.removeRole(Role.CONTENT_MANAGER);
    }

    @Test
    public void ensureRemoveRoleSucceeds() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        user.removeRole(Role.CLIENT);
        assertFalse(user.hasRole(Role.CLIENT));
    }

    @Test
    public void ensureHasRoleSucceeds() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        assertTrue(user.hasRole(Role.CLIENT));
    }

    @Test
    public void ensureHasRoleReturnsFalseIfUserDoesNotHaveRole() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        assertFalse(user.hasRole(Role.CONTENT_MANAGER));
    }

    @Test
    public void ensureIdWorks() {
        Auth auth = new UsernameCredentialsAuth("username", "password");
        User user = new User(auth);
        assertEquals(auth, user.id());
    }

    @Test
    public void ensureHashCodeWorks() {
        Auth auth = new UsernameCredentialsAuth("username", "password");
        User user = new User(auth);
        assertEquals(auth.hashCode(), user.hashCode());
    }

    @Test
    public void ensureEqualsReturnsTrueIfSameInstance() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        assertTrue(user.equals(user));
    }

    @Test
    public void ensureEqualsReturnsFalseIfArgumentIsNull() {
        User user = new User(new UsernameCredentialsAuth("username", "password"));
        assertFalse(user.equals(null));
    }

    @Test
    public void ensureEqualsReturnsFalseIfArgumentNotSameClass() {
        Auth auth = new UsernameCredentialsAuth("username", "password");
        User user = new User(auth);
        assertFalse(user.equals(auth));
    }

    @Test
    public void ensureEqualsReturnsFalseIfInstancesNotEqual() {
        Auth auth = new UsernameCredentialsAuth("username", "password");
        User user = new User(auth);
        Auth auth2 = new UsernameCredentialsAuth("username2", "password2");
        User user2 = new User(auth2);
        assertFalse(user.equals(user2));
    }

    @Test
    public void ensureEqualsReturnsTrueIfInstancesEqual() {
        Auth auth = new UsernameCredentialsAuth("username", "password");
        User user = new User(auth);
        User user2 = new User(auth);
        assertTrue(user.equals(user2));
    }
}
