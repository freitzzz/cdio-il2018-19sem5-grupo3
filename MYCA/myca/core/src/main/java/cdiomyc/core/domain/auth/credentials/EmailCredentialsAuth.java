package cdiomyc.core.domain.auth.credentials;

import cdiomyc.core.application.Application;
import cdiomyc.core.domain.auth.Auth;
import cdiomyc.support.encryptions.OperatorsEncryption;
import javax.persistence.Embedded;
import javax.persistence.Entity;

/**
 * Represents an email credentials based authentication
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Entity
public class EmailCredentialsAuth extends Auth{
    /**
     * Email with the credentials email
     */
    @Embedded
    private Email email;
    /**
     * Password with the credentials password
     */
    @Embedded
    private Password password;
    
    /**
     * Builds a new CredentialsAuth
     * @param email String with the credentials email
     * @param password String with the credentials password
     */
    public EmailCredentialsAuth(String email,String password){
        super(generateToken(Email.valueOf(email).email,Password.valueOf(password).password));
        this.email=Email.valueOf(email);
        this.password=Password.valueOf(password);
    }
    
    /**
     * Generates an authentication token based on credentials
     * @param email String with the credentials email
     * @param password String with the credentials password
     * @return String with the generated authentication token
     */
    private static String generateToken(String email,String password){
        return OperatorsEncryption
                .encrypt(email,
                        Application.settings().getEmailOperatorsEncryptionAlgorithm(),
                        Application.settings().getPasswordOperatorsEncryptionValue());
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected EmailCredentialsAuth(){}
}
