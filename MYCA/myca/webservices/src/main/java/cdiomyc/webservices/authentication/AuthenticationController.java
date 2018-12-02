package cdiomyc.webservices.authentication;

import cdiomyc.core.domain.User;
import cdiomyc.core.domain.auth.credentials.CredentialsAuth;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.AuthenticationMVService;
import cdiomyc.core.mv.authentication.session.GetAuthenticationSessionDetailsMV;
import cdiomyc.core.persistence.PersistenceContext;
import com.google.gson.Gson;
import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.core.Response.Status;

/**
 * Framework controller that processes authentication requests
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Path(value = "/auth")
public final class AuthenticationController {
    
    /**
     * Processes the authenticate into MYC API's request
     * @param authenticationDetails String with the authentication 
     * details
     * @return HTTP Response 200; OK with the API token
     * <br>    HTTP Response 401; Unauthorized if the authentication is invalid
     */
    @POST
    @Consumes(value = MediaType.APPLICATION_JSON)
    @Produces(value = MediaType.APPLICATION_JSON)
    public Response authenticate(String authenticationDetails){
        try{
            GetAuthenticationSessionDetailsMV authenticationSessionDetailsMV=
                    cdiomyc.core.application.auth.AuthenticationController
                            .authenticate(deserializeAuthenticationDetails(authenticationDetails));
            return Response
                    .ok()
                    .entity(new Gson().toJson(authenticationSessionDetailsMV))
                    .build();
        }catch(IllegalArgumentException|IllegalStateException invalidOperation){
            return Response
                    .status(Status.BAD_REQUEST)
                    .entity(new Gson().toJson(invalidOperation.getMessage()))
                    .build();
        }
    }
    
    /**
     * Deserializes the authentication details to the respective model view
     * @param authenticationDetails String with the authentication details
     * @return AuthenticationMV with the deserialized authentication model view
     */
    private static AuthenticationMV deserializeAuthenticationDetails(String authenticationDetails){
        return (AuthenticationMV)new Gson()
                .fromJson(authenticationDetails
                        ,AuthenticationMVService
                                .classFromType(new Gson()
                                        .fromJson(authenticationDetails,AuthenticationType.class)
                                            .type));
    }
    private void asdd(){
        try{
            System.out.println("!!!!!!!!!!!!1");
            User user=new User(new CredentialsAuth("superusername","superusername"));
            System.out.println(user);
            PersistenceContext.repositories().createUserRepository().save(user);
            System.out.println("!!!!1");
        }catch(Exception e){
            e.printStackTrace();
        }
    }
    /**
     * Simple inner static class to deserialize the type of an authentication request body
     */
    private static class AuthenticationType{
        public String type;
    }
}
