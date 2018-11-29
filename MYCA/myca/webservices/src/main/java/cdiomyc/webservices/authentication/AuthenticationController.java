package cdiomyc.webservices.authentication;

import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.AuthenticationMVService;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import java.lang.reflect.Type;
import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

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
    public Response authenticate(String authenticationDetails){
        throw new UnsupportedOperationException();
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
    /**
     * Simple inner static class to deserialize the type of an authentication request body
     */
    private static class AuthenticationType{
        public String type;
    }
}
