package cdiomyc.core.mv.users;

import cdiomyc.core.domain.Role;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import java.util.List;

/**
 * Model View representation for the add roles to user functionality
 * @author freitas
 */
public class AddUserRolesMV {
    
    /**
     * AuthenticationMV with the user authentication
     */
    public AuthenticationMV userAuthentication;
    
    /**
     * List with the user roles
     */
    public List<Role> userRoles;
}
