package cdiomyc.webservices.sms.mv.nexmo;

/**
 * Model View representation for the send SMS to receptor response details 
 * using Nexmo carrying service
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class NexmoSMSSendResponseMV {
    
    /**
     * Constant that represents the successful response status code given by Nexmo API
     */
    public static final short SUCCESSFUL_RESPONSE_STATUS_CODE=0;
    
    /**
     * String with the Nexmo SMS response statu
     */
    public short status;
}
