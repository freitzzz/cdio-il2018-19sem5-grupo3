package cdiomyc.webservices.sms.mv.nexmo;

import cdiomyc.webservices.sms.mv.SMSSendDetailsMV;

/**
 * Service for creating and manipulation NexmoSMS model views
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public class NexmoSMSMVService {
    
    public static NexmoSMSSendDetailsMV from(SMSSendDetailsMV smsSendDetailsMV){
        NexmoSMSSendDetailsMV nexmoSMSSendDetailsMV=new NexmoSMSSendDetailsMV();
        nexmoSMSSendDetailsMV.senderIdentifier=smsSendDetailsMV.senderIdentifier;
        nexmoSMSSendDetailsMV.message=smsSendDetailsMV.message;
        nexmoSMSSendDetailsMV.receptorPhoneNumber=smsSendDetailsMV.receptorPhoneNumber;
        nexmoSMSSendDetailsMV.apiKey=null;
        nexmoSMSSendDetailsMV.apiSecret=null;
        //TODO: CALL CONFIGURATION SERVICE FOR NEXMO API KEYS INJECTED VIA DEPENDECY INJECTION
        return nexmoSMSSendDetailsMV;
    }
}
