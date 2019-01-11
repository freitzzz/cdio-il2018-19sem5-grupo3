package cdiomyc.core.domain.auth.credentials;

import cdiomyc.core.application.Application;
import cdiomyc.support.domain.ddd.ValueObject;
import cdiomyc.support.encryptions.DigestUtils;
import cdiomyc.support.encryptions.OperatorsEncryption;
import java.io.Serializable;
import javax.persistence.Column;
import javax.persistence.Embeddable;

/**
 * Represents a simple email that can be used to authenticate on credentials
 * type systems
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Embeddable
public class Email implements Serializable,ValueObject {
    /**
     * Constant that represents the message that occurs if an email is invalid
     */
    private static final String INVALID_EMAIL="The email is invalid!";
    /**
     * Constant that represents the regular expression used to validate an email
     */
    private static final String EMAIL_REGEX_VALIDATOR="[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,64}";
    /**
     * String that represents the email value
     */
    @Column(length = 1<<14)
    protected String email;
    
    /**
     * Creates a new Email
     * @param email String with the email value
     * @return Email with the created Email
     */
    public static Email valueOf(String email){return new Email(email);}
    
    /**
     * Builds a new Email
     * @param email String with the email value
     */
    private Email(String email){
        checkEmail(email);
        this.email=encryptEmail(email);
    }
    
    /**
     * Returns the hashcode of the value object
     * @return Integer with the hash code of the value object
     */
    @Override
    public int hashCode(){return email.hashCode();}
    
    /**
     * Checks if a value object is equal to the current one
     * @param otherValueObject ValueObject with the comparing value object
     * @return boolean true if both value objects are equal, false if not
     */
    @Override
    public boolean equals(Object otherValueObject){return otherValueObject instanceof Email && ((Email)otherValueObject).email.equals(email);}
    
    /**
     * Returns the textual representation of the value object
     * @return String with the textual representation of the value object
     */
    @Override
    public String toString(){return email;};
    
    /**
     * Encrypts a email
     * @param email String with the email being encrypted
     * @return String with the encrypted email
     */
    private String encryptEmail(String email){
        String hashedEmail
                =DigestUtils
                        .hashify(email,
                                Application.settings().getEmailAlgorithm(),
                                Application.settings().getEmailSalt().getBytes());
        String encryptedHashedEmail
                =OperatorsEncryption
                        .encrypt(hashedEmail,
                                Application.settings().getEmailOperatorsEncryptionAlgorithm(),
                                Application.settings().getEmailOperatorsEncryptionValue());
        return encryptedHashedEmail;
    }
    
    /**
     * Checks if an email is valid
     * @param email String with the email being checked
     */
    private void checkEmail(String email){
        if(email==null||!email.matches(EMAIL_REGEX_VALIDATOR))
            throw new IllegalArgumentException(INVALID_EMAIL);
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected Email(){}
}
