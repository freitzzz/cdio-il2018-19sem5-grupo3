package cdiomyc.webservices.sms.mv.nexmo;

import cdiomyc.webservices.configuration.WebservicesConfiguration;
import cdiomyc.webservices.sms.mv.SMSSendDetailsMV;

/**
 * Service for creating and manipulation NexmoSMS model views
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class NexmoSMSMVService {
    
    /**
     * Transforms a SMSSendDetailsMV in a NexmoSMSSendDetailsMV using webservices configuration
     * @param smsSendDetailsMV SMSSendDetailsMV with the sms send details
     * @return NexmoSMSSendDetails with the Nexmo sms send details
     */
    public static NexmoSMSSendDetailsMV from(SMSSendDetailsMV smsSendDetailsMV){
        NexmoSMSSendDetailsMV nexmoSMSSendDetailsMV=new NexmoSMSSendDetailsMV();
        nexmoSMSSendDetailsMV.from=smsSendDetailsMV.senderIdentifier;
        nexmoSMSSendDetailsMV.text=smsSendDetailsMV.message;
        nexmoSMSSendDetailsMV.to=smsSendDetailsMV.receptorPhoneNumber;
        nexmoSMSSendDetailsMV.apiKey=WebservicesConfiguration.settings().getNexmoAPIKey();
        nexmoSMSSendDetailsMV.apiSecret=WebservicesConfiguration.settings().getNexmoAPISecret();
        return nexmoSMSSendDetailsMV;
    }
}
