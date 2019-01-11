package cdiomyc.webservices.sms.nexmo;

import cdiomyc.webservices.sms.exceptions.SMSSendException;
import cdiomyc.webservices.sms.mv.nexmo.NexmoSMSSendDetailsMV;
import cdiomyc.webservices.sms.mv.nexmo.NexmoSMSSendResponseMV;
import com.google.gson.Gson;
import javax.ws.rs.client.ClientBuilder;
import javax.ws.rs.client.Entity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

/**
 * Services class for sending SMS via Nexmo carrying service
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class NexmoSMSSenderService {
    
    /**
     * Constant that represents the carrier identifier for the SMS service
     */
    public static final String CARRIER="Nexmo";
    
    /**
     * Sends a message to a receptor via SMS using Nexmo carrying service
     * @param smsSendDetailsMV NexmoSMSSendDetailsMV with the message send details
     */
    public static void send(NexmoSMSSendDetailsMV smsSendDetailsMV){
        String deserializedSMSSendDetails=new Gson().toJson(smsSendDetailsMV,NexmoSMSSendDetailsMV.class);
        Response smsSendResponse=
                ClientBuilder
                        .newClient()
                        .target("https://rest.nexmo.com/sms/json")
                        .request(MediaType.APPLICATION_JSON)
                        .post(Entity
                                .json(deserializedSMSSendDetails));
        //TODO: Verify POST status code? Or trust Nexmo API response body status
        
        String nexmoSMSSendResponse=smsSendResponse.readEntity(String.class);
        
        //TODO: FIX NEXMO SEND RESPONSE SERIALIZATION
        
        NexmoSMSSendResponseMV nexmoSMSSendResponseMV=new Gson().fromJson(nexmoSMSSendResponse,NexmoSMSSendResponseMV.class);
        if(nexmoSMSSendResponseMV.status!=NexmoSMSSendResponseMV.SUCCESSFUL_RESPONSE_STATUS_CODE)
            throw new SMSSendException("An error occurd while sendind the SMS to the receptor");
    }
}
