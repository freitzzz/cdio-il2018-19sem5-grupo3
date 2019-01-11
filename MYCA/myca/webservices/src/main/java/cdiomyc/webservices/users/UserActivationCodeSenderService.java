package cdiomyc.webservices.users;

import cdiomyc.webservices.sms.mv.SendUserActivationCodeSMSDetailsMV;
import cdiomyc.webservices.configuration.WebservicesConfiguration;
import cdiomyc.webservices.emails.EmailSenderService;
import cdiomyc.webservices.emails.mv.SendUserActivationCodeEmailDetailsMV;
import cdiomyc.webservices.emails.sendgrid.EmailSendDetailsMV;
import cdiomyc.webservices.sms.SMSSenderService;
import cdiomyc.webservices.sms.mv.SMSSendDetailsMV;

/**
 * Service for sending user activation codes to the respective users
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class UserActivationCodeSenderService {
    
    /**
     * Sends the user account activation code to the user
     * @param sendUserActivationCodeDetailsMV SendUserActivationCodeDetailsMV with the user activation code send details
     */
    public static void sendUserActivationCode(SendUserActivationCodeDetailsMV sendUserActivationCodeDetailsMV){
        
        String activationCodeMessage=String.format("Hi %s ! "
                    + "\n Use this code when activating your account %s "
                    + "\n We hope you have fun using our services 🤠",
                    sendUserActivationCodeDetailsMV.name,
                    sendUserActivationCodeDetailsMV.activationCode);
        
        if(sendUserActivationCodeDetailsMV instanceof SendUserActivationCodeSMSDetailsMV){
            SMSSendDetailsMV smsSendDetailsMV=new SMSSendDetailsMV();
            smsSendDetailsMV.message=activationCodeMessage;
            smsSendDetailsMV.receptorPhoneNumber=((SendUserActivationCodeSMSDetailsMV) sendUserActivationCodeDetailsMV).phoneNumber;
            smsSendDetailsMV.carrier=WebservicesConfiguration.settings().getCurrentSMSCarrier();
            smsSendDetailsMV.senderIdentifier="MYC";
            SMSSenderService.send(smsSendDetailsMV);
        }else if(sendUserActivationCodeDetailsMV instanceof SendUserActivationCodeEmailDetailsMV){
            EmailSendDetailsMV emailSendDetailsMV=new EmailSendDetailsMV();
            emailSendDetailsMV.carrier=WebservicesConfiguration.settings().getCurrentEmailCarrier();
            emailSendDetailsMV.message=activationCodeMessage.replaceAll("\n","<br>");
            emailSendDetailsMV.title="Activation Code 🔑";
            emailSendDetailsMV.receptorsEmails=new String[]{((SendUserActivationCodeEmailDetailsMV) sendUserActivationCodeDetailsMV).email};
            EmailSenderService.send(emailSendDetailsMV);
        }
    }
    
}
