package cdiomyc.webservices.authentication;

import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;
import com.google.gson.Gson;
import javax.ws.rs.client.Entity;
import javax.ws.rs.core.Application;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import org.glassfish.jersey.server.ResourceConfig;
import org.glassfish.jersey.test.JerseyTest;
import org.glassfish.jersey.test.inmemory.InMemoryTestContainerFactory;
import org.glassfish.jersey.test.spi.TestContainerException;
import org.glassfish.jersey.test.spi.TestContainerFactory;
import org.junit.Assert;
import org.junit.Test;

/**
 * Integration tests for MYCA authentication functionalities
 * @author freitas
 */
public class AuthenticationIT extends JerseyTest{
    
    /**
     * Configures the Jersey framework to watch MYCA authentication functionalities
     * @return Application with the configured Jersey framework for MYCA authentication functionalities
     */
    @Override
    protected Application configure() {
        return new ResourceConfig(AuthenticationController.class);
    }
    
    /**
     * Configures the container launching MYCA server
     * @return TestContainerFactory TestContainerFactory with the container factory 
     * which will launch MYCA server
     * @throws TestContainerException if an error occurs during the container setup
     */
    @Override
    protected TestContainerFactory getTestContainerFactory() throws TestContainerException {
        return new InMemoryTestContainerFactory();
        
    }
    
    /**
     * Ensures that it's not possible authenticate an user with an invalid authentication type
     */
    @Test
    public void ensureCantAuthenticateUserWithInvalidAuthenticationType(){
        //In this test we will ensure that its not possible to authenticate an user 
        //which authentication type is invalid
        AuthenticationMV authenticationMV=new CredentialsAuthenticationMV();
        authenticationMV.type="this type of authentication is invalid";
        Response response=target("/auth")
            .request(MediaType.APPLICATION_JSON)
            .post(Entity.json(new Gson().toJson(authenticationMV)));
        int requestStatusCode=response.getStatus();
        Assert.assertEquals(Response.Status.UNAUTHORIZED.getStatusCode(),requestStatusCode);
    }
}
