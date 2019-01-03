package cdiomyc.core.domain.auth.credentials;

import cdiomyc.support.domain.ddd.ValueObject;
import java.io.Serializable;
import javax.persistence.Embeddable;

/**
 * Represents a simple name that can be used to identify a person 
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Embeddable
public class Name implements Serializable,ValueObject {
    /**
     * Constant that represents the message that occurs if a name is invalid
     */
    private static final String INVALID_NAME="The name is invalid!";
    /**
     * Constant that represents the regular expression used to validate a name
     */
    private static final String NAME_REGEX_VALIDATOR="([A-z]{3,}[ ]?)([A-z][ ]?)*";
    /**
     * String that represents the name value
     */
    protected String name;
    
    /**
     * Creates a new Name
     * @param name String with the name value
     * @return Name with the created Name
     */
    public static Name valueOf(String name){return new Name(name);}
    
    /**
     * Builds a new Name
     * @param name String with the name value
     */
    private Name(String name){
        checkName(name);
        this.name=encryptName(name);
    }
    
    /**
     * Returns the hashcode of the value object
     * @return Integer with the hash code of the value object
     */
    @Override
    public int hashCode(){return name.hashCode();}
    
    /**
     * Checks if a value object is equal to the current one
     * @param otherValueObject ValueObject with the comparing value object
     * @return boolean true if both value objects are equal, false if not
     */
    @Override
    public boolean equals(Object otherValueObject){return otherValueObject instanceof Name && ((Name)otherValueObject).name.equals(name);}
    
    /**
     * Returns the textual representation of the value object
     * @return String with the textual representation of the value object
     */
    @Override
    public String toString(){return name;};
    
    /**
     * Encrypts a name
     * @param name String with the name being encrypted
     * @return String with the encrypted name
     */
    private String encryptName(String name){
//        String hashedName
//                =DigestUtils
//                        .hashify(name,
//                                Application.settings().getNameAlgorithm(),
//                                Application.settings().getNameSalt().getBytes());
//        String encryptedHashedName
//                =OperatorsEncryption
//                        .encrypt(hashedName,
//                                Application.settings().getNameOperatorsEncryptionAlgorithm(),
//                                Application.settings().getNameOperatorsEncryptionValue());
//        return encryptedHashedName;
        return name;
    }
    
    /**
     * Checks if a name is valid
     * @param name String with the name being checked
     */
    private void checkName(String name){
        if(name==null||!name.matches(NAME_REGEX_VALIDATOR))
            throw new IllegalArgumentException(INVALID_NAME);
    }
    
    /**
     * Protected constructor to allow JPA persistence
     */
    protected Name(){}
}
