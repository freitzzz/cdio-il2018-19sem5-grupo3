package cdiomyc.core.domain;

import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.domain.auth.Session;
import cdiomyc.support.domain.ddd.AggregateRoot;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.OneToMany;
import javax.persistence.OneToOne;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;

/**
 * Represents an user that can be authenticated on MYC systems
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Entity
@SequenceGenerator(name = "userSeq",initialValue = 1,allocationSize = 1)
@Table(name = "MYCA_USER")
public class User implements AggregateRoot<Auth>,Serializable{
    /**
     * Constant that represents the default session time (in minuntes)
     */
    private static final int DEFAULT_SESSION_TIME=60;
    /**
     * Long with user persistence identifier
     */
    @Id
    @GeneratedValue(generator = "userSeq",strategy = GenerationType.SEQUENCE)
    private long id;
    
    /**
     * Auth with the user authentication
     */
    @OneToOne
    private Auth auth;
    
    /**
     * List with the user API sessions
     */
    @OneToMany(fetch = FetchType.LAZY)
    private List<Session> sessions;
    
    /**
     * Builds a new User
     * @param auth Auth with the user authentication
     */
    public User(Auth auth){
        checkAuth(auth);
        this.auth=auth;
        this.sessions=new ArrayList<>();
    }
    
    /**
     * Creates a new session
     * @return Session with the new user session
     */
    public Session createNewSession(){
        if(hasActiveSession())
            throw new IllegalArgumentException("User already has an active session!");
        Session createdSession=new Session(LocalDateTime.now().plusMinutes(DEFAULT_SESSION_TIME)); 
        this.sessions.add(createdSession);
        return createdSession;
    }
    
    /**
     * Returns the current user identifier
     * @return Auth with the user identifier
     */
    @Override
    public Auth id() {
        return auth;
    }
    
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
    public boolean equals(Object otherDomainEntity){return otherDomainEntity instanceof User && ((User)otherDomainEntity).id().equals(id());}
    
    /**
     * Checks if the current user has an active session
     * @return boolean true if the user has an active session, false if not
     */
    private boolean hasActiveSession(){
        return this.sessions.isEmpty()
                ? false
                : this.sessions
                        .get(this.sessions.size())
                        .isActive();
    }
    
    /**
     * Checks if an user authentication is valid
     * @param auth Auth with the user authentication
     */
    private void checkAuth(Auth auth){
        if(auth==null)
            throw new IllegalArgumentException("Invalid user authentication!");
    }
    
    /**
     * Protected constructor in order to allow JPA persistence
     */
    protected User(){}
}
