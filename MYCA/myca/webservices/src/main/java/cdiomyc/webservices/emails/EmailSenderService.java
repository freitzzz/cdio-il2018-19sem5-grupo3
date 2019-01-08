package cdiomyc.webservices.emails;

import cdiomyc.webservices.emails.mv.sendgrid.SendGridEmailMVService;
import cdiomyc.webservices.emails.mv.sendgrid.SendGridEmailSenderService;
import cdiomyc.webservices.emails.sendgrid.EmailSendDetailsMV;

/**
 * Service class for sending emails
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class EmailSenderService {
    
    /**
     * Sends an email to a group of receptors
     * @param emailSendDetailsMV EmailSendDetailsMV with the email details
     */
    public static void send(EmailSendDetailsMV emailSendDetailsMV){
        switch(emailSendDetailsMV.carrier){
            case SendGridEmailSenderService.CARRIER:
                SendGridEmailSenderService.send(SendGridEmailMVService.from(emailSendDetailsMV));
                break;
            default:
                throw new IllegalArgumentException("Invalid email carrier");
        }
    }
}
