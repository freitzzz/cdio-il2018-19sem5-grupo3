package cdiomyc.webservices.authentication;

import cdiomyc.core.mv.authentication.AuthenticationMV;
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
     * @param authenticationModelView AuthenticationMV with the authentication 
     * details
     * @return HTTP Response 200; OK with the API token
     * <br>    HTTP Response 401; Unauthorized if the authentication is invalid
     */
    @POST
    @Consumes(value = MediaType.APPLICATION_JSON)
    public Response authenticate(AuthenticationMV authenticationModelView){
        throw new UnsupportedOperationException();
    }
}
