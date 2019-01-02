package cdiomyc.bootstrapp.users;

import cdiomyc.bootstrapp.users.UsersBootstrapper.BootstrapperUser;
import cdiomyc.core.application.users.UserController;
import cdiomyc.core.mv.users.CreateCredentialsUserMV;
import cdiomyc.core.mv.users.CreatedUserMV;
import cdiomyc.core.mv.users.UserMVService;
import cdiomyc.support.actions.Action;
import com.google.gson.Gson;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.util.ArrayList;
import java.util.List;

/**
 * Represents MYCA Users bootstrapper
 * @author freitas
 */
public final class UsersBootstrapper implements Action{
    
    /**
     * Executes MYCA Users boostrapper
     */
    @Override
    public void execute() {
        try {
            usersToBoostrap().forEach((userToBootstrap) -> {
                CreatedUserMV createdUserMV=UserController.createUser(userToBootstrap);
                UserController.activateUser(UserMVService.fromUserMVToActivateUserMV(userToBootstrap,createdUserMV.activationCode));
                UserController.addUserRoles(UserMVService.fromUserMVToAddUserRolesMV(userToBootstrap,userToBootstrap.roles));
            });
        } catch (FileNotFoundException fileNotFoundException) {
            throw new IllegalStateException(fileNotFoundException);
        }
    }
    
    /**
     * Returns the users to boostrap
     * @return BootstrapperUsers with the users to boostrap
     * @throws FileNotFoundException Throws FileNotFoundException if users to boostrap file was not found 
     */
    private BootstrapperUsers usersToBoostrap() throws FileNotFoundException{
        return new Gson()
                .fromJson(new FileReader(
                    new File(UsersBootstrapper
                            .class
                            .getClassLoader()
                            .getResource("users.json").getPath())),
                    BootstrapperUsers.class);
    }
    
    /**
     * Inner class for helping bootstrapped users deserialization
     */
    protected class BootstrapperUsers extends ArrayList<BootstrapperUser>{}
    
    /**
     * Represents a user to boostrap details
     */
    protected class BootstrapperUser extends CreateCredentialsUserMV{
        
        /**
         * User roles
         */
        public List<String> roles;
    }
}
