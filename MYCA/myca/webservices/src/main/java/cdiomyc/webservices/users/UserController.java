package cdiomyc.webservices.users;

import cdiomyc.core.mv.users.CreateCredentialsUserMV;
import cdiomyc.core.mv.users.CreateUserMV;
import com.google.gson.Gson;
import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;

/**
 * Webservices UserController class
 * @author João
 */
public class UserController {
    /**
     * Creates an User
     * @param userCreationDetails
     * @return 
     */
    @POST
    @Consumes(value = MediaType.APPLICATION_JSON)
    public Response createUser(String userCreationDetails) {
        CreateUserMV createUserMV;
        //parse according to creation type
        new Gson().fromJson(userCreationDetails, CreateCredentialsUserMV.class);
        //new UserController().createUser(createUserMV);
        return null;
    }
}
