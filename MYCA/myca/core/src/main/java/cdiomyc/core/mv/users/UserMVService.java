package cdiomyc.core.mv.users;

import cdiomyc.core.domain.Role;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;
import cdiomyc.support.utils.JWTUtils;
import java.util.ArrayList;
import java.util.List;

/**
 * Service for creating and manipulating User Model Views
 *
 * @author João
 */
public class UserMVService {

    /**
     * Returns a model view class based on the create user type
     *
     * @param type String with the authentication type
     * @return Class with the respective authentication model view class
     * @throws IllegalArgumentException Throws IllegalArgumentException if the
     * authentication type is invalid
     */
    public static Class<?> classFromType(String type) {
        if (type == null || type.trim().isEmpty()) {
            throw new IllegalArgumentException("Invalid authentication type!");
        }
        switch (type) {
            case CredentialsAuthenticationMV.TYPE:
                return CreateCredentialsUserMV.class;
            default:
                throw new IllegalArgumentException("Invalid authentication type!");
        }
    }

    /**
     * Creates an instance of CreatedUserMV from an instance of Auth
     *
     * @param auth instance of Auth to aid in the creation
     * @return instance of CreatedUserMV
     */
    public static CreatedUserMV createdUserMVFromAuth(Auth auth) {
        CreatedUserMV createdUserMV = new CreatedUserMV();
        createdUserMV.token = JWTUtils.encode(auth.id());
        return createdUserMV;
    }
    
    /**
     * Creates an ActivateUserMV based on the user to authenticate and his activation code
     * @param authenticationMV AuthenticationMV with the user authentication details
     * @param activationCode String with the user activation code
     * @return ActivateUserMV with the user to activate details
     */
    public static ActivateUserMV fromUserMVToActivateUserMV(AuthenticationMV authenticationMV,String activationCode){
        ActivateUserMV activateUserMV=new ActivateUserMV();
        activateUserMV.activationCode=activationCode;
        activateUserMV.userAuthentication=authenticationMV;
        return activateUserMV;
    }
    
    /**
     * Creates an AddUserRolesMV based on the user to add roles and his new roles
     * @param authenticationMV AuthenticationMV with the user authentication details
     * @param roles List with the new user roles
     * @return AddUserRolesMV with the add roles to user details
     */
    public static AddUserRolesMV fromUserMVToAddUserRolesMV(AuthenticationMV authenticationMV,List<String> roles){
        AddUserRolesMV addUserRolesMV=new AddUserRolesMV();
        List<Role> userRoles=new ArrayList<>(roles.size());
        roles.forEach(role->{
            Role[] existingRoles=Role.values();
            for(int i=0;i<existingRoles.length;i++){
                if(existingRoles[i].name().equals(role) || existingRoles[i].toString().equals(role)){
                    userRoles.add(existingRoles[i]);
                    i=existingRoles.length;
                }
            }
        });
        addUserRolesMV.userAuthentication=authenticationMV;
        addUserRolesMV.userRoles=userRoles;
        return addUserRolesMV;
    }
}
