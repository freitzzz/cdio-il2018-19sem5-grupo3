package cdiomyc.core.domain.auth;

import java.io.Serializable;
import javax.persistence.Entity;
import javax.persistence.Id;

/**
 * Represents a base token authentication
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Entity
public abstract class Auth implements Serializable {
    /**
     * String with the authentication token
     */
    @Id
    private String token;
    
    /**
     * Builds a new Auth
     * @param token String with the authentication token
     */
    public Auth(String token){
        checkToken(token);
        this.token=token;
    }
    
    /**
     * Checks if an authentication token is invalid
     * @param token String with the authentication token being checked
     */
    private void checkToken(String token){
        if(token==null||token.trim().isEmpty())
            throw new IllegalArgumentException("The authentication token is invalid!");
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected Auth(){}
}
