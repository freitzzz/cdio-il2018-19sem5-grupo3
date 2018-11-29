package cdiomyc.core.domain.auth;

import java.io.Serializable;
import java.time.LocalDateTime;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;

/**
 * Represents an authentication session
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Entity
@SequenceGenerator(name = "sessionSeq",initialValue = 1, allocationSize = 1)
@Table(name = "MYCA_USER_SESSION")
public class Session implements Serializable{
    
    /**
     * Constant that represents the message that occurs if the session being created 
     * has an invalid end date time
     */
    private static final String INVALID_SESSION_END_DATE_TIME="Invalid session end date time!";
    /**
     * Long with the session persistence identifier
     */
    @Id
    @GeneratedValue(generator = "sessionSeq",strategy = GenerationType.SEQUENCE)
    private long id;
    /**
     * LocalDateTime with the session start date time
     */
    private LocalDateTime sessionStartDateTime;
    /**
     * LocalDateTime with the sesion end date time
     */
    private LocalDateTime sessionEndDateTime;
    /**
     * String with the session token
     */
    private String sessionToken;
    
    /**
     * Builds a new session
     * @param sessionEndDateTime LocalDateTime with the session end date time 
     */
    public Session(LocalDateTime sessionEndDateTime){
        LocalDateTime sessionStart=LocalDateTime.now();
        checkSessionEndDateTime(sessionStart,sessionEndDateTime);
        this.sessionStartDateTime=sessionStart;
        this.sessionEndDateTime=sessionEndDateTime;
    }
    
    /**
     * Checks if the current session is active
     * @return boolean true if the session is active, false if not
     */
    public boolean isActive(){return this.sessionEndDateTime.isAfter(this.sessionStartDateTime);}
    
    /**
     * Checks if the session end date time is valid
     * @param sessionStartDateTime LocalDateTime with the session start date time
     * @param sessionEndDateTime LocalDateTime with the session end date time
     */
    private void checkSessionEndDateTime(LocalDateTime sessionStartDateTime,LocalDateTime sessionEndDateTime){
        if(sessionEndDateTime==null||sessionEndDateTime.isBefore(sessionStartDateTime))
            throw new IllegalArgumentException(INVALID_SESSION_END_DATE_TIME);
    }
    
    /**
     * Protected constructor in order to allow JPA persistence
     */
    protected Session(){}
}
