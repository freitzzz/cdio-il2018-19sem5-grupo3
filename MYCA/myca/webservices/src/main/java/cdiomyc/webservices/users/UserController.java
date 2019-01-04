package cdiomyc.webservices.users;

import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.users.ActivateUserMV;
import cdiomyc.core.mv.users.CreateCredentialsUserMV;
import cdiomyc.core.mv.users.CreateUserMV;
import cdiomyc.core.mv.users.CreatedUserMV;
import cdiomyc.core.mv.users.UserMVService;
import cdiomyc.webservices.authentication.AuthenticationController;
import cdiomyc.webservices.dataservices.json.SimpleJSONMessageService;
import com.google.gson.Gson;
import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
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
            CreatedUserMV created = new cdiomyc.core.application.users.UserController().createUser(createUserMV);
            if(createUserMV instanceof CreateCredentialsUserMV){
                if(((CreateCredentialsUserMV) createUserMV).phoneNumber!=null && !((CreateCredentialsUserMV) createUserMV).phoneNumber.trim().isEmpty()){
                    SendUserActivationCodeSMSDetails sendUserActivationCodeSMSDetails=new SendUserActivationCodeSMSDetails();
                    sendUserActivationCodeSMSDetails.name=((CreateCredentialsUserMV) createUserMV).username;
                    sendUserActivationCodeSMSDetails.phoneNumber=((CreateCredentialsUserMV) createUserMV).phoneNumber;
                    sendUserActivationCodeSMSDetails.activationCode=created.activationCode;
                    UserActivationCodeSenderService.sendUserActivationCode(sendUserActivationCodeSMSDetails);
                }
            }
            return Response.ok().entity(new Gson().toJson(created)).build();
        } catch(IllegalArgumentException | IllegalStateException illegalArgumentException){
            return Response.status(Status.BAD_REQUEST).entity(new Gson().toJson(new SimpleJSONMessageService(illegalArgumentException.getMessage()))).build();
        } catch(Exception notCapturedException){
            return Response.status(Status.INTERNAL_SERVER_ERROR).encoding(new Gson().toJson(new SimpleJSONMessageService("An internal error has occurd"))).build();
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
            return Response.status(Status.INTERNAL_SERVER_ERROR).entity(new Gson().toJson(new SimpleJSONMessageService("An internal error has occurd"))).build();
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
