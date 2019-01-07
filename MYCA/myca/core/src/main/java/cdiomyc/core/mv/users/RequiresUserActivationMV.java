package cdiomyc.core.mv.users;

/**
 * Model View representation for the requires user activation operation
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class RequiresUserActivationMV {
    
    /**
     * Boolean to tell the user that his account requires to be activated
     */
    public boolean requiresActivation=true;
    
    /**
     * String with a custom message for the user
     */
    public String message="User activation is required";
}
