package cdiomyc.core.mv.users;

/**
 * CreatedUserMV class
 *
 * @author João
 */
public class CreatedUserMV {

    /**
     * Token of the created user
     */
    public String token;
    
    /**
     * String with the created user activation code
     */
    public String activationCode;
    
    /**
     * String with the created user name
     */
    public String name;
    
    /**
     * Boolean if the user details was sent via email to the user
     */
    public boolean sentDetailsViaEmail;
    
    /**
     * Boolean if the user details was sent via SMS to the user
     */
    public boolean sentDetailsViaSMS;
}
