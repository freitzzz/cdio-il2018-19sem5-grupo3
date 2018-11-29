package cdiomyc.core.domain;

import cdiomyc.core.domain.auth.Auth;
import java.io.Serializable;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
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
public class User implements Serializable{
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
     * Builds a new User
     * @param auth Auth with the user authentication
     */
    public User(Auth auth){
        checkAuth(auth);
        this.auth=auth;
    }
    
    /**
     * Checks if an user authentication is valid
     * @param auth Auth with the user authentication
     */
    private void checkAuth(Auth auth){
        if(auth==null)
            throw new IllegalArgumentException("Invalid user authentication!");
    }
    
    protected User(){}
}
