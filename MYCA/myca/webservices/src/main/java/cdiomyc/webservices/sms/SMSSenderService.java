package cdiomyc.webservices.sms;

import cdiomyc.webservices.sms.mv.SMSSendDetailsMV;
import cdiomyc.webservices.sms.mv.nexmo.NexmoSMSMVService;
import cdiomyc.webservices.sms.nexmo.NexmoSMSSenderService;

/**
 * Service class for sending SMS
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class SMSSenderService {
    
    /**
     * Sends a SMS to a receptor
     * @param smsSendDetailsMV SMSSendDetailsMV with the SMS details
     */
    public static void send(SMSSendDetailsMV smsSendDetailsMV){
        switch(smsSendDetailsMV.carrier){
            case NexmoSMSSenderService.CARRIER:
                NexmoSMSSenderService.send(NexmoSMSMVService.from(smsSendDetailsMV));
                break;
            default:
                throw new IllegalArgumentException("Invalid SMS carrier");
        }
    }
}
