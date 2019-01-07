package cdiomyc.webservices.emails.mv.sendgrid;

import com.google.gson.Gson;
import javax.ws.rs.client.ClientBuilder;
import javax.ws.rs.client.Entity;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

/**
 *
 * @author freitas
 */
public class SendGridEmailSenderService {
    
    /**
     * Constant that represents the carrier identifier for the email service
     */
    public static final String CARRIER="SendGrid";
    
    /**
     * Sends a message to a receptor via email using SendGrid carrying service
     * @param emailSendDetailsMV SendGridEmailSendDetailsMV with the message send details
     */
    public static void send(SendGridEmailSendDetailsMV emailSendDetailsMV){
        String deserializedSMSSendDetails=new Gson().toJson(emailSendDetailsMV,SendGridEmailSendDetailsMV.class);
        Response emailSendResponse=
                ClientBuilder
                        .newClient()
                        .target("https://api.sendgrid.com/v3/mail/send")
                        .request(MediaType.APPLICATION_JSON)
                        .header("Authorization","Bearer "+emailSendDetailsMV.sendGridAPIKey)
                        .post(Entity
                                .json(deserializedSMSSendDetails));
        
        if(emailSendResponse.getStatus()!=Response.Status.ACCEPTED.getStatusCode())
            throw new IllegalStateException("An error occurd while sendind the SMS to the receptor");
    }
    
}
