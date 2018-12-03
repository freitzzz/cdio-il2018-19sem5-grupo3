package cdiomyc.core.domain.auth;

import cdiomyc.support.domain.ddd.DomainEntity;
import java.io.Serializable;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.Inheritance;
import javax.persistence.InheritanceType;
import javax.persistence.SequenceGenerator;

/**
 * Represents a base token authentication
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Entity
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@SequenceGenerator(name = "authSeq",initialValue = 1, allocationSize = 1)

public abstract class Auth implements DomainEntity<String>,Serializable {
    /**
     * Long with the authentication persistence identifier
     */
    @Id
    @GeneratedValue(generator = "authSeq",strategy = GenerationType.SEQUENCE)
    private long id;
    /**
     * String with the authentication token
     */
    @Column(unique = true)
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
     * Returns the current auth identifier
     * @return String with the auth identifier
     */
    @Override
    public String id(){return token;}
    
    /**
     * Returns the hashcode of the domain entity
     * @return Integer with the hash code of the domain entity
     */
    @Override
    public int hashCode(){return id().hashCode();}
    
    /**
     * Checks if a domain entity is equal to the current one
     * @param otherDomainEntity DomainEntity with the comparing domain entity
     * @return boolean true if both domain entities are equal, false if not
     */
    @Override
    public boolean equals(Object otherDomainEntity){return otherDomainEntity instanceof Auth && ((Auth)otherDomainEntity).id().equals(id());}
    
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
