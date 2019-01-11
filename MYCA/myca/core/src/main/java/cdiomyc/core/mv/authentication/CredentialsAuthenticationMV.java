package cdiomyc.core.mv.authentication;

/**
 * Model View representation for the authenticate into MYC APIs operation using 
 * user credentials
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class CredentialsAuthenticationMV extends AuthenticationMV{
    /**
     * Constant that represents the authentication model view type
     */
    public static final String TYPE="credentials";
    /**
     * String with the credentials username
     */
    public String username;
    /**
     * String with the credentials password
     */
    public String password;
}
