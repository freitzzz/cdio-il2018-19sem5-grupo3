package cdiomyc.core.mv.users;

import cdiomyc.core.mv.authentication.AuthenticationMV;

/**
 * Model View representation for the activate user functionality
 * @author freitas
 */
public class ActivateUserMV{
    
    /**
     * String with the user activation code
     */
    public String activationCode;
    
    /**
     * AuthenticationMV with the user authentication
     */
    public AuthenticationMV userAuthentication;
}
