package cdiomyc.core.application.grants;

import cdiomyc.core.domain.auth.Session;

/**
 * Service for granting certain user session operations
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class SessionGrants {
    
    /**
     * Grants that a user session is active
     * @param session Session with the user session being granted
     */
    public static void grantSessionIsActive(Session session){
        grantSessionIsValid(session);
        if(!session.isActive())
            throw new IllegalStateException("Session is not active!");
    }
    
    /**
     * Grants that a user session has a token
     * @param session Session with the user session being granted
     * @param sessionAPIToken String with the user session token
     */
    public static void grantSessionHasToken(Session session,String sessionAPIToken){
        grantSessionIsValid(session);
        if(!session.sameAs(sessionAPIToken))
            throw new IllegalStateException("Session is invalid!");
    }
    
    /**
     * Grants that a user session is valid
     * @param session Session with the user session being granted
     */
    private static void grantSessionIsValid(Session session){
        if(session==null)
            throw new IllegalStateException("Session is not valid!");
    }
}
