package cdiomyc.webservices.dataservices.json;

/**
 * Service for representing simple JSON messages
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class SimpleJSONMessageService {
    
    /**
     * String with the message being represented
     */
    public String message;
    
    /**
     * Builds a new SimpleJSONMessageService
     * @param message String with the message being represented
     */
    public SimpleJSONMessageService(String message){this.message=message;}
}
