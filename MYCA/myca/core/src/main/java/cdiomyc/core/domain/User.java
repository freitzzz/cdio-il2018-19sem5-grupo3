package cdiomyc.core.domain;

import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.domain.auth.Session;
import cdiomyc.core.domain.exceptions.UserNotEnabledException;
import cdiomyc.support.domain.ddd.AggregateRoot;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Random;
import java.util.Set;
import java.util.UUID;
import javax.persistence.CascadeType;
import javax.persistence.Embedded;
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
     * Constant that represents the default session time (in minutes)
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
    @OneToOne(cascade = CascadeType.PERSIST)
    private Auth auth;
    
    /**
     * Name with the user name
     */
    @Embedded
    private Name name;
    
    /**
     * List with the user API sessions
     */
    @OneToMany(cascade = CascadeType.PERSIST,fetch = FetchType.LAZY)
    private List<Session> sessions;
    
    /**
     * Set with the user roles
     */
    private Set<Role> roles;
    
    /**
     * Short with the user activation code
     */
    private short activationCode;
    
    /**
     * Boolean with the user enableness
     */
    private boolean enabled;
    
    /**
     * Builds a new User
     * @param auth Auth with the user authentication
     */
    public User(Auth auth){
        checkAuth(auth);
        this.auth=auth;
        this.sessions=new ArrayList<>();
        this.roles=new HashSet<>();
        this.roles.add(Role.CLIENT);
        this.enabled=false;
        this.activationCode=generateActivationCode();
    }
    
    //Needs unit tests to be updated
    
    /**
     * Changes the current user name
     * @param name String with the new user name
     */
    public void changeName(String name){
        Name newUserName=Name.valueOf(name);
        if(newUserName.equals(this.name))
            throw new IllegalArgumentException("Both old and new user names are equal");
        this.name=newUserName;
    }
    
    /**
     * Returns the current user name
     * @return String with the user name
     */
    public String name(){return name!=null ? name.name : "Anonymous";}
    
    /**
     * Creates a new session
     * @return Session with the new user session
     */
    public Session createNewSession(){return createNewSession(UUID.randomUUID().toString());}
    
    /**
     * Creates a new session using a secrete identifier
     * @param secreteIdentifier String with the user session secrete identifier
     * @return Session with the new user session
     */
    public Session createNewSession(String secreteIdentifier){
        grantUserIsEnabled();
//        if(hasActiveSession())
//            throw new IllegalArgumentException("User already has an active session!");
        Session createdSession=new Session(LocalDateTime.now().plusMinutes(DEFAULT_SESSION_TIME),auth.id()
                ,secreteIdentifier); 
        this.sessions.add(createdSession);
        return createdSession;
    }
    
    /**
     * Ends the current user session
     */
    public void endSession(){
        getLastSession().deactivate();
    }
    
    /**
     * Returns the user last session
     * @return Session with the user last session
     */
    public Session getLastSession(){
        return !sessions.isEmpty() ? sessions.get(sessions.size()-1) : null;
    }
    
    /**
     * Adds a new role to the user
     * @param role Role with the role being added to the user
     */
    public void addRole(Role role){
        checkRole(role);
        if(this.roles.contains(role))
            throw new IllegalStateException(String.format("User already has the role %s !",role));
        this.roles.add(role);
    }
    
    /**
     * Adds a set of roles to the user
     * @param roles Iterable with the roles being added to the user
     */
    public void addRoles(Iterable<Role> roles){
        if(roles==null||!roles.iterator().hasNext())
            throw new IllegalArgumentException("Roles to add are invalid");
        roles.forEach(role->{this.addRole(role);});
    }
    
    /**
     * Removes a role from the user
     * @param role Role with the role being removed
     */
    public void removeRole(Role role){
        checkRole(role);
        if(!this.roles.contains(role))
            throw new IllegalStateException(String.format("User does not have the role %s !",role));
        this.roles.remove(role);
    }
    
    /**
     * Checks if a user has a certain role
     * @param role Role with the role being checked
     * @return boolean true if the user has a certain role, false if not
     */
    public boolean hasRole(Role role){
        checkRole(role);
        return this.roles.contains(role);
    }
    
    //TODO: Requires Unit Tests updates
    
    /**
     * Activates the current user
     * @param activationCode String with the user activation code
     */
    public void activate(String activationCode){
        if(this.enabled)
            throw new IllegalStateException("User is already enabled");
        if(this.activationCode!=Short.parseShort(activationCode))
            throw new IllegalArgumentException("Activation code is invalid!");
        this.enabled=true;
    }
    
    public String newActivationCode(){
        if(this.enabled)
            throw new IllegalStateException("User is already enabled");
        this.activationCode=generateActivationCode();
        return activationCode();
    }
    
    /**
     * Returns the current user activation code
     * @return String with the user activation code
     */
    public String activationCode(){return this.activationCode>999 ? Short.toString(this.activationCode) : String.format("0%s",this.activationCode);}
    
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
                        .get(this.sessions.size()-1)
                        .isActive();
    }
    
    /**
     * Generates a random activation code
     * @return Short with the generated activation code
     */
    private short generateActivationCode(){return (short)new Random().nextInt(9999);}
    
    /**
     * Checks if an user authentication is valid
     * @param auth Auth with the user authentication being checked
     */
    private void checkAuth(Auth auth){
        if(auth==null)
            throw new IllegalArgumentException("Invalid user authentication!");
    }
    
    /**
     * Checks if a user role is valid
     * @param role Role with the role being checked
     */
    private void checkRole(Role role){
        if(role==null)
            throw new IllegalArgumentException("Role is invalid!");
    }
    
    /**
     * Grants that the currrent user is enabled
     */
    private void grantUserIsEnabled(){
        if(!this.enabled)
            throw new UserNotEnabledException("User is not enabled");
    }
    
    /**
     * Protected constructor in order to allow JPA persistence
     */
    protected User(){}
    
    /**
     * Buider class for simplifying the creation of a user
     */
    public static class UserBuilder{
        
        /**
         * User with the user being build
         */
        private final User userBeingBuild;
        
        /**
         * Builds a new UserBuilder
         * @param auth Auth with the user auth
         */
        private UserBuilder(Auth auth){this.userBeingBuild=new User(auth);}
        
        /**
         * Creates a new UserBuilder
         * @param auth Auth with the user auth
         * @return UserBuilder with the created UserBuilder
         */
        public static UserBuilder createUserBuilder(Auth auth){
            return new UserBuilder(auth);
        }
        
        /**
         * Adds a name to the user being build
         * @param name String with the user name
         * @return UserBuilder with the refreshed user builder
         */
        public UserBuilder withName(String name){
            this.userBeingBuild.changeName(name);
            return this;
        }
        
        /**
         * Builds the user
         * @return User with the built user
         */
        public User build(){return userBeingBuild;}
        
    }
    
}
