package cdiomyc.webservices.authorization;

import cdiomyc.core.mv.authorization.IsUserAuthorizedMV;
import cdiomyc.webservices.dataservices.json.SimpleJSONMessageService;
import com.google.gson.Gson;
import javax.ws.rs.GET;
import javax.ws.rs.HeaderParam;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.core.Response.Status;

/**
 * Framework controller that processes authorization requests
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
@Path(value = "/autho")
public class AuthorizationController {
    /**
     * Processes the check if an user is authorized to perform any action in the MYC API's request
     * @param sessionAPIToken String with the user session API token
     * @param asContentManager boolean true if the user is a content manager
     * @return HTTP Response 204; No Content if the user is authorized
     * <br>    HTTP Response 401; Unauthorized if the user is not authorized
     */
    @GET
    @Produces(value = MediaType.APPLICATION_JSON)
    public Response isAuthorized(@HeaderParam(value = "Authorization: Basic ")String sessionAPIToken, 
            @QueryParam(value = "contentmanager") boolean asContentManager){
        IsUserAuthorizedMV userAuthorizationDetails=new IsUserAuthorizedMV();
        userAuthorizationDetails.sessionAPIToken=sessionAPIToken;
        userAuthorizationDetails.isContentManager=asContentManager;
        try{
            cdiomyc.core.application.autho.AuthorizationController.isAuthorized(userAuthorizationDetails);
            return Response.noContent().build();
        }catch(IllegalArgumentException|IllegalStateException notAuthorizedException){
            return Response
                    .status(Status.BAD_REQUEST)
                    .entity(new Gson().toJson(new SimpleJSONMessageService(notAuthorizedException.getMessage())))
                    .build();
        }catch(Exception internalErrorException){
            return Response
                    .status(Status.INTERNAL_SERVER_ERROR)
                    .entity(new Gson().toJson(new SimpleJSONMessageService("An internal error has occurd :(")))
                    .build();
        }
    }
}
