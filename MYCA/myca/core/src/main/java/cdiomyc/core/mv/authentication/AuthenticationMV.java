package cdiomyc.core.mv.authentication;

/**
 * Model View representation for the authenticate into MYC APIs operation
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public abstract class AuthenticationMV {
    /**
     * String with the authentication User-Agent
     */
    public String secreteKey;
    /**
     * String with the authentication secrete key
     */
    public String userAgent;
    /**
     * String with the authentication type
     */
    public String type;
}
