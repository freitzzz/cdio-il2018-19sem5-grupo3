package cdiomyc.webservices.users.exceptions;

/**
 * Exception that is thrown when an error occurs in a send service
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class SendException extends IllegalStateException{
    
    /**
     * Builds a new SendException
     * @param message String with the exception cause
     */
    public SendException(String message){super(message);}
}
