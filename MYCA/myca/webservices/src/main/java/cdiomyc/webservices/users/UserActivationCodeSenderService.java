package cdiomyc.webservices.users;

import cdiomyc.webservices.configuration.WebservicesConfiguration;
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
        if(sendUserActivationCodeDetailsMV instanceof SendUserActivationCodeSMSDetails){
            SMSSendDetailsMV smsSendDetailsMV=new SMSSendDetailsMV();
            smsSendDetailsMV.message=String.format("Hi %s ! "
                    + "\n Use this code when activating your account %s "
                    + "\n We hope you have fun using our services 🤠",
                    ((SendUserActivationCodeSMSDetails) sendUserActivationCodeDetailsMV).name,
                    ((SendUserActivationCodeSMSDetails) sendUserActivationCodeDetailsMV).activationCode);
            smsSendDetailsMV.receptorPhoneNumber=((SendUserActivationCodeSMSDetails) sendUserActivationCodeDetailsMV).phoneNumber;
            smsSendDetailsMV.carrier=WebservicesConfiguration.settings().getCurrentSMSCarrier();
            smsSendDetailsMV.senderIdentifier="MYC";
            SMSSenderService.send(smsSendDetailsMV);
        }
    }
    
}
