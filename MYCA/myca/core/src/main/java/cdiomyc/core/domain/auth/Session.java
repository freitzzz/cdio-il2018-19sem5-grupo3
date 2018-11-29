package cdiomyc.core.domain.auth;

import java.time.LocalDateTime;
import javax.persistence.Embeddable;

/**
 * Represents an authentication session
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Embeddable
public class Session {
    /**
     * Constant that represents the message that occurs if the session being created 
     * has an invalid end date time
     */
    private static final String INVALID_SESSION_END_DATE_TIME="Invalid session end date time!";
    /**
     * LocalDateTime with the session start date time
     */
    private LocalDateTime sessionStartDateTime;
    /**
     * LocalDateTime with the sesion end date time
     */
    private LocalDateTime sessionEndDateTime;
    
    /**
     * Creates a new session with a specified end date time
     * @param sessionEndDateTime LocalDateTime with the session end date time
     * @return Session with the created session
     */
    public static Session valueOf(LocalDateTime sessionEndDateTime){
        return new Session(sessionEndDateTime);
    }
    
    /**
     * Builds a new session
     * @param sessionEndDateTime LocalDateTime with the session end date time 
     */
    private Session(LocalDateTime sessionEndDateTime){
        LocalDateTime sessionStart=LocalDateTime.now();
        checkSessionEndDateTime(sessionStart,sessionEndDateTime);
        this.sessionStartDateTime=sessionStart;
        this.sessionEndDateTime=sessionEndDateTime;
    }
    
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
