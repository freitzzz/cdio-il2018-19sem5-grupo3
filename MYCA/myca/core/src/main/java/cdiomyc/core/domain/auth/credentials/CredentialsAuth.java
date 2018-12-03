package cdiomyc.core.domain.auth.credentials;

import cdiomyc.core.application.Application;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.support.encryptions.OperatorsEncryption;
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
        super(generateToken(Username.valueOf(username).username,Password.valueOf(password).password));
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
        return OperatorsEncryption
                .encrypt(username.concat(password),
                        Application.settings().getUsernameOperatorsEncryptionAlgorithm(),
                        Application.settings().getPasswordOperatorsEncryptionValue());
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected CredentialsAuth(){}
}
