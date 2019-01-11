package cdiomyc.webservices.sms.exceptions;

import cdiomyc.webservices.users.exceptions.SendException;

/**
 * Exception for when an error occurs while sending a SMS
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class SMSSendException extends SendException{
    
    /**
     * Builds a new SMSSendException
     * @param message String with the sms send exception cause
     */
    public SMSSendException(String message){super(message);}
}
