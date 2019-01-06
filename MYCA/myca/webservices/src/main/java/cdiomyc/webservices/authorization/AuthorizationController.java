package cdiomyc.webservices.authorization;

import cdiomyc.core.mv.authorization.IsUserAuthorizedMV;
import cdiomyc.webservices.cookieservices.SessionCookieService;
import cdiomyc.webservices.dataservices.json.SimpleJSONMessageService;
import com.google.gson.Gson;
import javax.ws.rs.CookieParam;
import javax.ws.rs.GET;
import javax.ws.rs.HeaderParam;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.QueryParam;
import javax.ws.rs.core.Cookie;
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
     * @param userAgent String with the user User Agent
     * @param secrete String with the user secrete identifier
     * @param sessionCookie String with the session cookie
     * @param asAdministrator boolean true if the user is an administrator
     * @param asContentManager boolean true if the user is a content manager
     * @param asLogisticManager boolean true if the user is a logistic manager
     * @return HTTP Response 204; No Content if the user is authorized
     * <br>    HTTP Response 401; Unauthorized if the user is not authorized
     */
    @GET
    @Produces(value = MediaType.APPLICATION_JSON)
    public Response isAuthorized(@HeaderParam(value="User-Agent") String userAgent,
            @HeaderParam(value = "Secrete") String secrete,
            @CookieParam(value = "MYCASESSION") String sessionCookie,
            @QueryParam(value = "administrator") boolean asAdministrator,
            @QueryParam(value = "contentmanager") boolean asContentManager,
            @QueryParam(value = "logisticmanager") boolean asLogisticManager){
        IsUserAuthorizedMV userAuthorizationDetails=new IsUserAuthorizedMV();
        Cookie userSessionCookie=SessionCookieService.toSessionCookie(sessionCookie);
        userAuthorizationDetails.sessionAPIToken=userSessionCookie.getValue();
        userAuthorizationDetails.isAdministrator=asAdministrator;
        userAuthorizationDetails.isContentManager=asContentManager;
        userAuthorizationDetails.isLogisticManager=asLogisticManager;
        userAuthorizationDetails.userAgent=userAgent;
        userAuthorizationDetails.secreteKey=secrete;
        try{
            cdiomyc.core.application.autho.AuthorizationController.isAuthorized(userAuthorizationDetails);
            return Response.noContent().build();
        }catch(IllegalArgumentException|IllegalStateException notAuthorizedException){
            return Response
                    .status(Status.UNAUTHORIZED)
                    .entity(new Gson().toJson(new SimpleJSONMessageService(notAuthorizedException.getMessage())))
                    .build();
        }catch(Exception internalErrorException){
            return Response
                    .status(Status.INTERNAL_SERVER_ERROR)
                    .entity(new Gson().toJson(new SimpleJSONMessageService("An internal error has occurd :(")))
                    .build();
        }
    }
    
    /**
     * Treats the authorization header
     * @param authorizationHeader String with the authorization header
     * @return String with the authorization header value
     */
    private static String treatAuthorizationHeader(String authorizationHeader){
        return authorizationHeader!=null 
                ? authorizationHeader.replace("Bearer ","")
                : null;
    }
}
