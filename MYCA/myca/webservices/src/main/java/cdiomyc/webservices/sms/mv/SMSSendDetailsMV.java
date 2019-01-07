package cdiomyc.webservices.sms.mv;

/**
 * Model View representation of the SMS details for the send SMS to receptor functionality
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class SMSSendDetailsMV {
    
    /**
     * String with the carrier to be used to send the SMS
     */
    public String carrier;
    
    /**
     * String with the sender identifier (Identifier of the message to send)
     */
    public String senderIdentifier;
    
    /**
     * String with the message to send to the receptor
     */
    public String message;
    
    /**
     * String with the receptor phone number (Receptor which will receive the message)
     */
    public String receptorPhoneNumber;
}
