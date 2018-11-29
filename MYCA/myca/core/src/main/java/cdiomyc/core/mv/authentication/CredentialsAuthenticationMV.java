package cdiomyc.core.mv.authentication;

/**
 * Model View representation for the authenticate into MYC APIs operation using 
 * user credentials
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class CredentialsAuthenticationMV extends AuthenticationMV{
    /**
     * String with the credentials username
     */
    public String username;
    /**
     * String with the credentials password
     */
    public String password;
}
