package cdiomyc.webservices.emails.sendgrid;

/**
 * Model View representation of the Email details for the send email to receptor functionality
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class EmailSendDetailsMV {
    
    /**
     * String with the carrier to be used to send the email
     */
    public String carrier;
    
    /**
     * String with the message to send to the receptor
     */
    public String message;
           
    /**
     * Array with the receptors emails (Receptor which will receive the message)
     */
    public String[] receptorsEmails;
    
    /**
     * String with the email title
     */
    public String title;
}
