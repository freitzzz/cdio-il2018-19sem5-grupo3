package cdiomyc.core.domain.auth;

import cdiomyc.support.domain.ddd.DomainEntity;
import cdiomyc.support.utils.JWTUtils;
import java.io.Serializable;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.UUID;
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
@SequenceGenerator(name = "sessionSeq", initialValue = 1, allocationSize = 1)
@Table(name = "MYCA_USER_SESSION")
public class Session implements DomainEntity<String>, Serializable {

    /**
     * Constant that represents the message that occurs if the session being
     * created has an invalid end date time
     */
    private static final String INVALID_SESSION_END_DATE_TIME = "Invalid session end date time!";
    
    /**
     * Constant that represents the message that occurs if the session token is invalid
     */
    private static final String INVALID_TOKEN = "Invalid token!";
    
    /**
     * Constant that represents the message that occurs if the session secrete identifier is invalid
     */
    private static final String INVALID_SECRETE_IDENTIFIER = "Invalid secrete identifier!";
    /**
     * Long with the session persistence identifier
     */
    @Id
    @GeneratedValue(generator = "sessionSeq", strategy = GenerationType.SEQUENCE)
    private long id;
    /**
     * LocalDateTime with the session start date time
     */
    private LocalDateTime sessionStartDateTime;
    /**
     * LocalDateTime with the session end date time
     */
    private LocalDateTime sessionEndDateTime;
    /**
     * String with the session token
     */
    private String sessionToken;
    /**
     * String with the session secrete identifier
     */
    private String sessionSecreteIdentifier;

    /**
     * Builds a new session
     * @param sessionEndDateTime LocalDateTime with the session end date time
     * @param authToken String with the user auth token
     * @param sessionSecreteIdentifier String with the session secrete identifier
     */
    public Session(LocalDateTime sessionEndDateTime, String authToken,String sessionSecreteIdentifier) {
        checkToken(authToken);
        checkSecreteIdentifier(authToken);
        LocalDateTime sessionStart = LocalDateTime.now();
        checkSessionEndDateTime(sessionStart, sessionEndDateTime);
        this.sessionStartDateTime = sessionStart;
        this.sessionEndDateTime = sessionEndDateTime;
        this.sessionToken = generateSessionToken(authToken);
        this.sessionSecreteIdentifier=sessionSecreteIdentifier;
    }
    
    /**
     * Checks if the current session is active
     * @return boolean true if the session is active, false if not
     */
    public boolean isActive() {
        return this.sessionEndDateTime.isAfter(this.sessionStartDateTime);
    }

    /**
     * Returns the session token as a JWT
     * @return String with the current session token as a JWT
     */
    public String tokenAsJWT() {
        return JWTUtils.encode(this.sessionToken);
    }

    /**
     * Returns the session end date time
     * @return LocalDateTime with the current session end date time
     */
    public LocalDateTime getSessionEndDateTime() {
        return sessionEndDateTime;
    }

    /**
     * Returns the current session identifier
     * @return String with the session identifier
     */
    @Override
    public String id() {
        return sessionToken;
    }

    /**
     * Returns the hashcode of the domain entity
     * @return Integer with the hash code of the domain entity
     */
    @Override
    public int hashCode() {
        return id().hashCode();
    }

    /**
     * Checks if a domain entity is equal to the current one
     * @param otherDomainEntity DomainEntity with the comparing domain entity
     * @return boolean true if both domain entities are equal, false if not
     */
    @Override
    public boolean equals(Object otherDomainEntity) {
        return otherDomainEntity instanceof Session && ((Session) otherDomainEntity).id().equals(id());
    }

    /**
     * Generates a user session API token
     * @param token String with the user auth token
     * @return String with the generated session API token
     */
    private String generateSessionToken(String token) {
        return UUID.randomUUID()
                .toString()
                .concat(token)
                .concat(LocalDateTime.now().format(DateTimeFormatter.ISO_DATE));
    }

    /**
     * Checks if the session end date time is valid
     * @param sessionStartDateTime LocalDateTime with the session start date
     * time
     * @param sessionEndDateTime LocalDateTime with the session end date time
     */
    private void checkSessionEndDateTime(LocalDateTime sessionStartDateTime, LocalDateTime sessionEndDateTime) {
        if (sessionEndDateTime == null || sessionEndDateTime.isBefore(sessionStartDateTime)) {
            throw new IllegalArgumentException(INVALID_SESSION_END_DATE_TIME);
        }
    }

    /**
     * Checks if token is valid
     * @param token String with the token
     */
    private void checkToken(String token) {
        if (token == null || token.trim().isEmpty()) {
            throw new IllegalArgumentException(INVALID_TOKEN);
        }
    }
    
    /**
     * Checks if a secrete ident
     * @param secreteIdentifier String with the token
     */
    private void checkSecreteIdentifier(String secreteIdentifier) {
        if (secreteIdentifier == null || secreteIdentifier.trim().isEmpty()) {
            throw new IllegalArgumentException(INVALID_SECRETE_IDENTIFIER);
        }
    }

    /**
     * Protected constructor in order to allow JPA persistence
     */
    protected Session() {}
}
