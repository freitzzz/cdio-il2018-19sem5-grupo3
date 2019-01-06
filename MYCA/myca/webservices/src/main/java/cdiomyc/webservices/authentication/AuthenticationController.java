package cdiomyc.webservices.authentication;

import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.AuthenticationMVService;
import cdiomyc.core.mv.authentication.session.GetAuthenticationSessionDetailsMV;
import cdiomyc.webservices.cookieservices.SessionCookieService;
import cdiomyc.webservices.dataservices.json.SimpleJSONMessageService;
import com.google.gson.Gson;
import javax.ws.rs.Consumes;
import javax.ws.rs.HeaderParam;
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
     * @param userAgent String with the user User-Agent
     * @param secrete String with hte user secrete key
     * @param authenticationDetails String with the authentication 
     * details
     * @return HTTP Response 200; OK with the API token
     * <br>    HTTP Response 401; Unauthorized if the authentication is invalid
     */
    @POST
    @Consumes(value = MediaType.APPLICATION_JSON)
    @Produces(value = MediaType.APPLICATION_JSON)
    public Response authenticate(@HeaderParam(value="User-Agent") String userAgent,
                @HeaderParam(value = "Secrete")String secrete,
                String authenticationDetails){
        try{
                System.out.println(authenticationDetails);
            AuthenticationMV authenticationDetailsMV=deserializeAuthenticationDetails(authenticationDetails);
            authenticationDetailsMV.userAgent=userAgent;
            authenticationDetailsMV.secreteKey=secrete;
            GetAuthenticationSessionDetailsMV authenticationSessionDetailsMV=
                    cdiomyc.core.application.auth.AuthenticationController
                            .authenticate(authenticationDetailsMV);
            return Response
                    .ok()
                    .cookie(SessionCookieService
                            .createSessionCookie(authenticationSessionDetailsMV.token))
                    .entity(new Gson().toJson(authenticationSessionDetailsMV))
                    .build();
        }catch(IllegalArgumentException|IllegalStateException invalidOperation){
            return Response
                    .status(Status.UNAUTHORIZED)
                    .entity(new Gson().toJson(new SimpleJSONMessageService(invalidOperation.getMessage())))
                    .build();
        }catch(InternalError internalError){
            return Response
                    .status(Status.INTERNAL_SERVER_ERROR)
                    .entity(new Gson().toJson(new SimpleJSONMessageService(internalError.getMessage())))
                    .build();
        }catch(Exception _internalError){
            return Response
                    .status(Status.INTERNAL_SERVER_ERROR)
                    .entity(new Gson().toJson(new SimpleJSONMessageService("An internal error has occurd :(")))
                    .build();
        }
    }
    
    /**
     * Deserializes the authentication details to the respective model view
     * @param authenticationDetails String with the authentication details
     * @return AuthenticationMV with the deserialized authentication model view
     */
    public static AuthenticationMV deserializeAuthenticationDetails(String authenticationDetails){
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
    public static class AuthenticationType{
        public String type;
    }
}
