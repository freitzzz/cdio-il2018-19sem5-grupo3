package cdiomyc.webservices.sms.mv;

import cdiomyc.webservices.users.SendUserActivationCodeDetailsMV;

/**
 * Model View representation for the send user activation code details using SMS services
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class SendUserActivationCodeSMSDetailsMV extends SendUserActivationCodeDetailsMV{
    
    /**
     * String with the user phone number
     */
    public String phoneNumber;
    
}
