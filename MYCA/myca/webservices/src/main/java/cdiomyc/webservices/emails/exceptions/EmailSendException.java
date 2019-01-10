package cdiomyc.webservices.emails.exceptions;

import cdiomyc.webservices.users.exceptions.SendException;

/**
 * Exception for when an error occurs while sending an email
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class EmailSendException extends SendException{
    
    /**
     * Builds a new EmailSendException
     * @param message String with the email send exception cause
     */
    public EmailSendException(String message){super(message);}
}
