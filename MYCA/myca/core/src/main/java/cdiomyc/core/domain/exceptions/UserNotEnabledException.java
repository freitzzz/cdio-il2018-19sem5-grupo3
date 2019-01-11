package cdiomyc.core.domain.exceptions;

/**
 * Represents an exception that is thrown when the user attemps to do an action 
 * when he is not enabled
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class UserNotEnabledException extends IllegalStateException{
    
    /**
     * Builds a new UserNotEnabledException
     * @param message String with the custom exception message
     */
    public UserNotEnabledException(String message){super(message);}
}
