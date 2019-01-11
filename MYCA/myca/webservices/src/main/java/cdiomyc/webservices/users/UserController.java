package cdiomyc.webservices.users;

import cdiomyc.webservices.sms.mv.SendUserActivationCodeSMSDetailsMV;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.users.ActivateUserMV;
import cdiomyc.core.mv.users.CreateCredentialsUserMV;
import cdiomyc.core.mv.users.CreateUserMV;
import cdiomyc.core.mv.users.CreatedUserMV;
import cdiomyc.core.mv.users.FindUserBySessionMV;
import cdiomyc.core.mv.users.UserDetailsMV;
import cdiomyc.core.mv.users.UserMVService;
import cdiomyc.webservices.authentication.AuthenticationController;
import cdiomyc.webservices.cookieservices.SessionCookieService;
import cdiomyc.webservices.dataservices.json.SimpleJSONMessageService;
import cdiomyc.webservices.emails.mv.SendUserActivationCodeEmailDetailsMV;
import cdiomyc.webservices.users.exceptions.SendException;
import com.google.gson.Gson;
import javax.ws.rs.Consumes;
import javax.ws.rs.CookieParam;
import javax.ws.rs.GET;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.Cookie;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.core.Response.Status;

/**
 * Webservices UserController class
 *
 * @author João
 */
@Path(value = "/users")
public class UserController {

    private static final String UNEXPECTED_ERROR_MESSAGE = "An unexpected error has occurred.";

    /**
     * Creates an User
     *
     * @param userCreationDetails
     * @return
     */
    @POST
    @Consumes(value = MediaType.APPLICATION_JSON)
    @Produces(value = MediaType.APPLICATION_JSON)
    public Response createUser(String userCreationDetails) {
        try {
            CreateUserMV createUserMV;
            createUserMV = (CreateUserMV) new Gson().fromJson(userCreationDetails, UserMVService.classFromType(new Gson().fromJson(userCreationDetails, CreateUserType.class).type));
            CreatedUserMV createdUserMV = cdiomyc.core.application.users.UserController.createUser(createUserMV);
            if(createUserMV instanceof CreateCredentialsUserMV){
                if(((CreateCredentialsUserMV) createUserMV).phoneNumber!=null && !((CreateCredentialsUserMV) createUserMV).phoneNumber.trim().isEmpty()){
                    SendUserActivationCodeSMSDetailsMV sendUserActivationCodeSMSDetails=new SendUserActivationCodeSMSDetailsMV();
                    sendUserActivationCodeSMSDetails.name=createdUserMV.name;
                    sendUserActivationCodeSMSDetails.phoneNumber=((CreateCredentialsUserMV) createUserMV).phoneNumber;
                    sendUserActivationCodeSMSDetails.activationCode=createdUserMV.activationCode;
                    createdUserMV.sentDetailsViaSMS=trySendUserActivationCode(sendUserActivationCodeSMSDetails);
                }
                
                if(((CreateCredentialsUserMV) createUserMV).email!=null && !((CreateCredentialsUserMV) createUserMV).email.trim().isEmpty()){
                    SendUserActivationCodeEmailDetailsMV sendUserActivationCodeEmailDetailsMV=new SendUserActivationCodeEmailDetailsMV();
                    sendUserActivationCodeEmailDetailsMV.email=((CreateCredentialsUserMV) createUserMV).email;
                    sendUserActivationCodeEmailDetailsMV.name=createdUserMV.name;
                    sendUserActivationCodeEmailDetailsMV.activationCode=createdUserMV.activationCode;
                    createdUserMV.sentDetailsViaEmail=trySendUserActivationCode(sendUserActivationCodeEmailDetailsMV);
                }
            }
            return Response.ok().entity(new Gson().toJson(createdUserMV)).build();
        } catch(IllegalArgumentException | IllegalStateException illegalArgumentException){
            return Response.status(Status.BAD_REQUEST).entity(new Gson().toJson(new SimpleJSONMessageService(illegalArgumentException.getMessage()))).build();
        } catch(Exception notCapturedException){
            return Response.status(Status.INTERNAL_SERVER_ERROR).encoding(new Gson().toJson(new SimpleJSONMessageService(UNEXPECTED_ERROR_MESSAGE))).build();
        }
    }
    
    /**
     * Activates an user
     * @param userActivationDetails String with the user activation details
     * @return HTTP Response 204; No Content if the user was activated with success
     * <br>    HTTP Response 400; Bad Request if the activation code  is invalid
     * <br>    HTTP Response 401; Not Authorized if the user authentication details are invalid or the user is already activated
     */
    @Path(value = "/activate")
    @POST
    @Consumes(value = MediaType.APPLICATION_JSON)
    @Produces(value = MediaType.APPLICATION_JSON)
    public Response activateUser(String userActivationDetails){
        try{
            ActivateUserMV activateUserMV=new Gson().fromJson(userActivationDetails,ActivateUserMV.class);
            AuthenticationMV authenticationMV=AuthenticationController.deserializeAuthenticationDetails(userActivationDetails);
            activateUserMV.userAuthentication=authenticationMV;
            cdiomyc.core.application.users.UserController.activateUser(activateUserMV);
            return Response.noContent().build();
        }catch(IllegalArgumentException illegalArgumentException){
            return Response.status(Status.BAD_REQUEST).entity(new Gson().toJson(new SimpleJSONMessageService(illegalArgumentException.getMessage()))).build();
        }catch(IllegalStateException illegalStateException){
            return Response.status(Status.UNAUTHORIZED).entity(new Gson().toJson(new SimpleJSONMessageService(illegalStateException.getMessage()))).build();
        }catch(Exception exception){
            return Response.status(Status.INTERNAL_SERVER_ERROR).entity(new Gson().toJson(new SimpleJSONMessageService(UNEXPECTED_ERROR_MESSAGE))).build();
        }
    }

    /**
     * Retrieves a User's details by providing a session cookie.
     * @return HTTP Response 200; OK with the UserDetailsMV
     * <br>    HTTP Response 400; Bad Request if the activation code  is invalid
     * <br>    HTTP Response 401; Not Authorized if the user authentication details are invalid or the user is already activated
     */
    @GET
    @Produces(value = MediaType.APPLICATION_JSON)
    public Response getUserAuthToken(@CookieParam(value = "MYCASESSION") String sessionCookie){
        try{
            if(sessionCookie == null || sessionCookie.trim().isEmpty()){
                return Response.status(Status.BAD_REQUEST).entity(new Gson().toJson(new SimpleJSONMessageService("No session cookie was provided"))).build();
            }

            Cookie userSessionCookie=SessionCookieService.toSessionCookie(sessionCookie);

            FindUserBySessionMV findUserBySessionMV = new FindUserBySessionMV();
            findUserBySessionMV.sessionToken = userSessionCookie.getValue();

            UserDetailsMV userDetailsMV = cdiomyc.core.application.users.UserController.getUserDetails(findUserBySessionMV);

            return Response.ok(new Gson().toJson(userDetailsMV)).build();
            
        }catch(IllegalArgumentException illegalArgumentException){
            return Response.status(Status.BAD_REQUEST).entity(new Gson().toJson(new SimpleJSONMessageService(illegalArgumentException.getMessage()))).build();
        }catch(IllegalStateException illegalStateException){
            return Response.status(Status.UNAUTHORIZED).entity(new Gson().toJson(new SimpleJSONMessageService(illegalStateException.getMessage()))).build();
        }catch(Exception exception){
            return Response.status(Status.INTERNAL_SERVER_ERROR).entity(new Gson().toJson(new SimpleJSONMessageService(UNEXPECTED_ERROR_MESSAGE))).build();
        }
    }
    
    /**
     * Tries to send the activation code to the user via a send (Email | SMS) service
     * @param sendUserActivationCodeDetailsMV SendUserActivationCodeDetailsMV with the user activation code send details
     * @return boolean true if the activation code send was successful, false if not
     */
    private boolean trySendUserActivationCode(SendUserActivationCodeDetailsMV sendUserActivationCodeDetailsMV){
        try{
            UserActivationCodeSenderService.sendUserActivationCode(sendUserActivationCodeDetailsMV);
            return true;
        }catch(SendException sendException){
            return false;
        }
    }
    
    /**
     * Simple inner static class to deserialize the type of an authentication
     * request body
     */
    private static class CreateUserType {

        public String type;
    }
}
