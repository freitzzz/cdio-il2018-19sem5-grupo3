package cdiomyc.core.domain.auth.credentials;

import java.io.Serializable;
import javax.persistence.Embeddable;

/**
 * Represents a simple password that can be used to authenticate on credentials
 * type systems
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Embeddable
public class Password implements Serializable {
    /**
     * Constant that represents the message that occurs if a password is invalid
     */
    private static final String INVALID_PASSWORD="The password is invalid!";
    /**
     * Constant that represents the regular expression used to validate a password
     */
    private static final String PASSWORD_REGEX_VALIDATOR="[a-zA-z0-9]{5,12}";
    /**
     * String that represents the password value
     */
    private String value;
    
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
        this.value=password;
    }
    
    /**
     * Checks if a password is valid
     * @param password String with the password being checked
     */
    private void checkPassword(String password){
        if(password==null||!password.matches(PASSWORD_REGEX_VALIDATOR))
            throw new IllegalArgumentException(INVALID_PASSWORD);
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected Password(){}
}