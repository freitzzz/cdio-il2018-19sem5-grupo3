package cdiomyc.core.application.users;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.domain.auth.AuthFactory;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.users.CreateUserMV;
import cdiomyc.core.mv.users.CreatedUserMV;
import cdiomyc.core.mv.users.UserMVService;
import cdiomyc.core.persistence.PersistenceContext;
import cdiomyc.core.persistence.UserRepository;

/**
 * Core UserController class
 *
 * @author João
 */
public class UserController {

    /**
     * Creates an User
     *
     * @param userCreationDetails MV containing the user creation details
     * @return instance of CreatedUserMV containing the auth token
     */
    public CreatedUserMV createUser(CreateUserMV userCreationDetails) {
        Auth auth = AuthFactory.createAuth((AuthenticationMV) userCreationDetails);
        UserRepository userRepo = PersistenceContext.repositories().createUserRepository();
        try {
            userRepo.findEID(auth);
            throw new IllegalArgumentException("User already exists!");
        } catch (IllegalArgumentException ex) {
            User user = new User(auth);
            userRepo.save(user);
            return UserMVService.createdUserMVFromAuth(auth);
        }
    }
}
