package cdiomyc.webservices.emails.mv.sendgrid;

import cdiomyc.webservices.configuration.WebservicesConfiguration;
import cdiomyc.webservices.emails.sendgrid.EmailSendDetailsMV;
import java.util.ArrayList;

/**
 * Service for creating and manipulating SendGrid model views
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class SendGridEmailMVService {
    
    /**
     * Transforms a EmailSendDetailsMV in a SendGridEmailSendDetailsMV using webservices configuration
     * @param emailSendDetailsMV EmailSendDetailsMV with the email send details
     * @return SendGridEmailSendDetails with the SendGrid email send details
     */
    public static SendGridEmailSendDetailsMV from(EmailSendDetailsMV emailSendDetailsMV){
        SendGridEmailSendDetailsMV sendGridEmailSendDetailsMV=new SendGridEmailSendDetailsMV();
        SendGridEmailSendDetailsMV.SendGridEmailPersonalization sendGridEmailPersonalization=new SendGridEmailSendDetailsMV.SendGridEmailPersonalization();
        //Email subject/title
        sendGridEmailPersonalization.subject=emailSendDetailsMV.title;
        
        //Email receptors
        sendGridEmailPersonalization.to=new ArrayList<>();
        for(int i=0;i<emailSendDetailsMV.receptorsEmails.length;i++){
            SendGridEmailSendDetailsMV.Email receptorEmail=new SendGridEmailSendDetailsMV.Email();
            receptorEmail.email=emailSendDetailsMV.receptorsEmails[i];
            sendGridEmailPersonalization.to.add(receptorEmail);
        }
        sendGridEmailSendDetailsMV.personalizations=new ArrayList<>();
        sendGridEmailSendDetailsMV.personalizations.add(sendGridEmailPersonalization);
        
        //From email (the email which is show to the user as the sent one)
        SendGridEmailSendDetailsMV.Email fromEmail=new SendGridEmailSendDetailsMV.Email();
        fromEmail.email="customerservice@myc.com";
        sendGridEmailSendDetailsMV.from=fromEmail;
        
        //Email content (message & type)
        SendGridEmailSendDetailsMV.EmailContent emailContent=new SendGridEmailSendDetailsMV.EmailContent();
        emailContent.message=emailSendDetailsMV.message;
        sendGridEmailSendDetailsMV.content=new ArrayList<>();
        sendGridEmailSendDetailsMV.content.add(emailContent);
        
        sendGridEmailSendDetailsMV.sendGridAPIKey=WebservicesConfiguration.settings().getSendGridAPIKey();
        
        return sendGridEmailSendDetailsMV;
    }
}
