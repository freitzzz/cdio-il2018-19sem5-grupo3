package cdiomyc.core.mv.authentication;

/**
 * Service for creating and manipulating Authentication Model Views
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class AuthenticationMVService {
    /**
     * Returns a model view class based on an authentication type
     * @param type String with the authentication type
     * @return Class with the respective authentication model view class 
     * @throws IllegalArgumentException Throws IllegalArgumentException if the 
     * authentication type is invalid
     */
    public static Class<?> classFromType(String type){
        switch(type){
            case CredentialsAuthenticationMV.TYPE:
                return CredentialsAuthenticationMV.class;
            default:
                throw new IllegalArgumentException("Invalid authentication type!");
        }
    }
}
