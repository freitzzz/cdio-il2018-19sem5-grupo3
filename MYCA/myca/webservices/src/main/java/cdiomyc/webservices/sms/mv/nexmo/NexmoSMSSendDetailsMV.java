package cdiomyc.webservices.sms.mv.nexmo;

import cdiomyc.webservices.sms.mv.SMSSendDetailsMV;
import com.google.gson.annotations.SerializedName;

/**
 * Model View representation of the SMS details for the send SMS to receptor functionality 
 * using Nexmo carrying service
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class NexmoSMSSendDetailsMV extends SMSSendDetailsMV{
    
    /**
     * String with the Nexmo API key
     */
    @SerializedName(value = "api_key")
    public String apiKey;
    
    /**
     * String with the Nexmo API secret
     */
    @SerializedName(value = "api_secret")
    public String apiSecret;
    
    /**
     * String with the Nexmo sender indentifier
     */
    @SerializedName(value = "from")
    public String from;
    
    /**
     * String with the Nexmo SMS message to send
     */
    @SerializedName(value = "text")
    public String text;
    
    /**
     * String with the Nexmo receptor phone number
     */
    @SerializedName(value = "to")
    public String to;
}
