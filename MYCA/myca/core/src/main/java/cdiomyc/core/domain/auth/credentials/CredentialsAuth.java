package cdiomyc.core.domain.auth.credentials;

import cdiomyc.core.domain.auth.Auth;
import javax.persistence.Embedded;
import javax.persistence.Entity;

/**
 * Represents a credentials based authentication
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Entity
public class CredentialsAuth extends Auth{
    /**
     * Username with the credentials username
     */
    @Embedded
    private Username username;
    /**
     * Password with the credentials password
     */
    @Embedded
    private Password password;
    
    /**
     * Builds a new CredentialsAuth
     * @param username String with the credentials username
     * @param password String with the credentials password
     */
    public CredentialsAuth(String username,String password){
        super(generateToken(username,password));
        this.username=Username.valueOf(username);
        this.password=Password.valueOf(password);
    }
    
    /**
     * Generates an authentication token based on credentials
     * @param username String with the credentials username
     * @param password String with the credentials password
     * @return String with the generated authentication token
     */
    private static String generateToken(String username,String password){
        throw new UnsupportedOperationException("#TODO");
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected CredentialsAuth(){}
}
