package cdiomyc.webservices.emails.mv.sendgrid;

import cdiomyc.webservices.emails.sendgrid.EmailSendDetailsMV;
import com.google.gson.annotations.SerializedName;
import java.util.List;

/**
 * Model View representation of the email details for the send email to group of receptors functionality 
 * using SendGrid carrying service
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class SendGridEmailSendDetailsMV extends EmailSendDetailsMV{
    
    /**
     * Map with the email personalizations (repectors + message)
     */
    @SerializedName(value = "personalizations")
    public List<SendGridEmailPersonalization> personalizations;
    
    /**
     * Receptor email with the from email
     */
    @SerializedName(value = "from")
    public Email from;
    
    /**
     * EmailContent with the email content
     */
    @SerializedName(value = "content")
    public List<EmailContent> content;
    
    /**
     * String with the SendGrid API key
     */
    public transient String sendGridAPIKey;
    
    /**
     * Static Inner class for representing send grid email personalizations
     */
    public static class SendGridEmailPersonalization{
        
        /**
         * List with the email receptors
         */
        @SerializedName(value = "to")
        public List<Email> to;
        
        /**
         * String with the email subjects
         */
        @SerializedName(value = "subject")
        public String subject;
    }
    
    /**
     * Static Inner class for representing receptor/from emails
     */
    public static class Email{
        
        /**
         * String with the receptor/from email
         */
        @SerializedName(value = "email")
        public String email;
    }
    
    /**
     * Static Inner class for representing the content of a SendGrid email
     */
    public static class EmailContent{
        
        /**
         * String with the email content type
         */
        @SerializedName(value = "type")
        public String type="text/html";
        
        /**
         * String with the email content message
         */
        @SerializedName(value = "value")
        public String message;
    }
}
