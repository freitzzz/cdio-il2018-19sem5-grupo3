package cdiomyc.webservices.cookieservices;

import javax.ws.rs.core.NewCookie;

/**
 * Service for creating and manipulating MYCA API session cookies
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class SessionCookieService {
    /**
     * Constant that represents the session cookie name
     */
    public static final String COOKIE_NAME="MYCASESSION";
    
    /**
     * Constant that represents the session cookie name identifier
     */
    private static final String COOKIE_NAME_IDENTIFIER=COOKIE_NAME+"=";
    
    /**
     * Creates a new session cookie
     * @param sessionToken String with the session token
     * @return NewCookie with the created session cookie
     */
    public static NewCookie createSessionCookie(String sessionToken){
        return new NewCookie(COOKIE_NAME,sessionToken);
    }
    
    /**
     * Converts a cookie as String representation to a NewCookie
     * @param sessionCookie String with the session cookie
     * @return NewCookie with the converted cookie
     */
    public static NewCookie toSessionCookie(String sessionCookie){
        if(sessionCookie==null)
            throw new IllegalArgumentException("Invalid Session Cookie");
        String realSessionCookie=sessionCookie.startsWith(COOKIE_NAME_IDENTIFIER) ? sessionCookie : COOKIE_NAME_IDENTIFIER.concat(sessionCookie);
        return NewCookie.valueOf(realSessionCookie);
    }
    
}
