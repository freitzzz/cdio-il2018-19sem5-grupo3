package cdiomyc.core.mv.users;

import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;

/**
 * CreateCredentialsUserMV class, represents the model/view used to transfer data used in creating an user by its credentials
 * @author João
 */
public class CreateCredentialsUserMV extends CredentialsAuthenticationMV implements CreateUserMV {

    /**
     * String with the user to create phone number
     */
    public String phoneNumber;
    
    /**
     * String with the user to create name
     */
    public String name;
    
}
