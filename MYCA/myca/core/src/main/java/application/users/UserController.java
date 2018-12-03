package application.users;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.domain.auth.AuthFactory;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.users.CreateCredentialsUserMV;
import cdiomyc.core.mv.users.CreateUserMV;
import cdiomyc.core.persistence.PersistenceContext;
import cdiomyc.core.persistence.UserRepository;

/**
 * Core UserController class
 *
 * @author João
 */
public class UserController {

    public void createUser(CreateUserMV userCreationDetails) {
        Auth auth = AuthFactory.createAuth((AuthenticationMV) userCreationDetails);
        UserRepository userRepo = PersistenceContext.repositories().createUserRepository();
        try {
            userRepo.findEID(auth);
            throw new IllegalArgumentException("User already exists!");
        } catch (IllegalArgumentException ex) {
            User user = new User(auth);
            userRepo.save(user);
        }
    }
}
