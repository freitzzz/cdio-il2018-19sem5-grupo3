package cdiomyc.core.mv.authentication.session;

/**
 * Model View representation for the authentication session details operation
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class GetAuthenticationSessionDetailsMV {
    /**
     * String with the authentication session token
     */
    public String token;
    /**
     * String with the authentication session end date time
     */
    public String sessionEnd;
}
