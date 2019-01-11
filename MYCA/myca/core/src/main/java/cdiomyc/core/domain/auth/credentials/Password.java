package cdiomyc.core.domain.auth.credentials;

import cdiomyc.core.application.Application;
import cdiomyc.support.domain.ddd.ValueObject;
import cdiomyc.support.encryptions.DigestUtils;
import cdiomyc.support.encryptions.OperatorsEncryption;
import java.io.Serializable;
import javax.persistence.Column;
import javax.persistence.Embeddable;

/**
 * Represents a simple password that can be used to authenticate on credentials
 * type systems
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Embeddable
public class Password implements Serializable,ValueObject {
    /**
     * Constant that represents the message that occurs if a password is invalid
     */
    private static final String INVALID_PASSWORD="The password is invalid!";
    /**
     * Constant that represents the regular expression used to validate a password
     */
    private static final String PASSWORD_REGEX_VALIDATOR="[a-zA-z0-9]{5,24}";
    /**
     * String that represents the password value
     */
    @Column(length = 1<<14)
    protected String password;
    
    /**
     * Creates a new Password
     * @param password String with the password value
     * @return Password with the created Password
     */
    public static Password valueOf(String password){return new Password(password);}
    
    /**
     * Builds a new Password
     * @param password String with the password value
     */
    private Password(String password){
        checkPassword(password);
        this.password=encryptPassword(password);
    }
    
    /**
     * Returns the hashcode of the value object
     * @return Integer with the hash code of the value object
     */
    @Override
    public int hashCode(){return password.hashCode();}
    
    /**
     * Checks if a value object is equal to the current one
     * @param otherValueObject ValueObject with the comparing value object
     * @return boolean true if both value objects are equal, false if not
     */
    @Override
    public boolean equals(Object otherValueObject){return otherValueObject instanceof Password && ((Password)otherValueObject).password.equals(password);}
    
    /**
     * Returns the textual representation of the value object
     * @return String with the textual representation of the value object
     */
    @Override
    public String toString(){return password;};
    
    /**
     * Checks if a password is valid
     * @param password String with the password being checked
     */
    private void checkPassword(String password){
        if(password==null||!password.matches(PASSWORD_REGEX_VALIDATOR))
            throw new IllegalArgumentException(INVALID_PASSWORD);
    }
    
    /**
     * Encrypts a password
     * @param password String with the password being encrypted
     * @return String with the encrypted password
     */
    private String encryptPassword(String password){
        String hashedPassword
                =DigestUtils
                        .hashify(password,
                                Application.settings().getPasswordAlgorithm(),
                                Application.settings().getPasswordSalt().getBytes());
        String encryptedHashedPassword
                =OperatorsEncryption
                        .encrypt(hashedPassword,
                                Application.settings().getPasswordOperatorsEncryptionAlgorithm(),
                                Application.settings().getPasswordOperatorsEncryptionValue());
        return encryptedHashedPassword;
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected Password(){}
}