package cdiomyc.core.domain.auth.credentials;

import cdiomyc.support.domain.ddd.ValueObject;
import java.io.Serializable;
import javax.persistence.Embeddable;

/**
 * Represents a simple username that can be used to authenticate on credentials
 * type systems
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Embeddable
public class Username implements Serializable,ValueObject {
    /**
     * Constant that represents the message that occurs if an username is invalid
     */
    private static final String INVALID_USERNAME="The username is invalid!";
    /**
     * Constant that represents the regular expression used to validate an username
     */
    private static final String USERNAME_REGEX_VALIDATOR="[a-zA-z0-9]{5,12}";
    /**
     * String that represents the username value
     */
    private String value;
    
    /**
     * Creates a new Username
     * @param username String with the username value
     * @return Username with the created Username
     */
    public static Username valueOf(String username){return new Username(username);}
    
    /**
     * Builds a new Username
     * @param username String with the username value
     */
    private Username(String username){
        checkUsername(username);
        this.value=username;
    }
    
    /**
     * Returns the hashcode of the value object
     * @return Integer with the hash code of the value object
     */
    @Override
    public int hashCode(){return value.hashCode();}
    
    /**
     * Checks if a value object is equal to the current one
     * @param otherValueObject ValueObject with the comparing value object
     * @return boolean true if both value objects are equal, false if not
     */
    @Override
    public boolean equals(Object otherValueObject){return otherValueObject instanceof Username && ((Username)otherValueObject).value.equals(value);}
    
    /**
     * Returns the textual representation of the value object
     * @return String with the textual representation of the value object
     */
    @Override
    public String toString(){return value;};
    
    /**
     * Checks if an username is valid
     * @param username String with the username being checked
     */
    private void checkUsername(String username){
        if(username==null||!username.matches(USERNAME_REGEX_VALIDATOR))
            throw new IllegalArgumentException(INVALID_USERNAME);
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected Username(){}
}
