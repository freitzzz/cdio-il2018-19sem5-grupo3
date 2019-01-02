package cdiomyc.core.application.users;

import cdiomyc.core.domain.Role;
import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.domain.auth.AuthFactory;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.users.ActivateUserMV;
import cdiomyc.core.mv.users.AddUserRolesMV;
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
    public static CreatedUserMV createUser(CreateUserMV userCreationDetails) {
        Auth auth = AuthFactory.createAuth((AuthenticationMV) userCreationDetails);
        UserRepository userRepo = PersistenceContext.repositories().createUserRepository();
        try {
            userRepo.findEID(auth);
        } catch (IllegalStateException ex) {
            User user = new User(auth);
            userRepo.save(user);
            CreatedUserMV createdUserMV=UserMVService.createdUserMVFromAuth(auth);
            createdUserMV.activationCode=user.activationCode();
            return createdUserMV;
        }
        throw new IllegalStateException("User already exists!");
    }
    
    /**
     * Activates an user
     * @param activateUserMV ActivateUserMV with the user to activate details 
     */
    public static void activateUser(ActivateUserMV activateUserMV){
        Auth userAuth=AuthFactory.createAuth(activateUserMV.userAuthentication);
        UserRepository userRepo=PersistenceContext.repositories().createUserRepository();
        User userToActivate=userRepo.findEID(userAuth);
        userToActivate.activate(activateUserMV.activationCode);
        userRepo.update(userToActivate);
    }
    
    /**
     * Adds roles to an user
     * @param addUserRolesMV AddUserRolesMV with the user new roles details 
     */
    public static void addUserRoles(AddUserRolesMV addUserRolesMV){
        Auth userAuth=AuthFactory.createAuth(addUserRolesMV.userAuthentication);
        UserRepository userRepo=PersistenceContext.repositories().createUserRepository();
        User userToAddRoles=userRepo.findEID(userAuth);
        userToAddRoles.addRoles(addUserRolesMV.userRoles);
        userRepo.update(userToAddRoles);
    }
}
