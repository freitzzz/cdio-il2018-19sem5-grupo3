package cdiomyc.core.domain.auth;

import java.io.Serializable;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.SequenceGenerator;

/**
 * Represents a base token authentication
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Entity
@SequenceGenerator(name = "authSeq",initialValue = 1, allocationSize = 1)
public abstract class Auth implements Serializable {
    /**
     * Long with the authentication persistence identifier
     */
    @Id
    @GeneratedValue(generator = "authSeq",strategy = GenerationType.SEQUENCE)
    private long id;
    /**
     * String with the authentication token
     */
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
