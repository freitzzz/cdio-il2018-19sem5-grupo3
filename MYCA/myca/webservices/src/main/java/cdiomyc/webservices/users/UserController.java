package cdiomyc.webservices.users;

import cdiomyc.core.mv.users.CreateUserMV;
import cdiomyc.core.mv.users.CreatedUserMV;
import cdiomyc.core.mv.users.UserMVService;
import com.google.gson.Gson;
import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

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
            return Response.ok().entity(new Gson().toJson(created)).build();
        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }
        return null;
    }

    /**
     * Simple inner static class to deserialize the type of an authentication
     * request body
     */
    private static class CreateUserType {

        public String type;
    }
}
