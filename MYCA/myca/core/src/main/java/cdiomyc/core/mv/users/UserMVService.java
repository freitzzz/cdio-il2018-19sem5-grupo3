package cdiomyc.core.mv.users;

import cdiomyc.core.domain.auth.Auth;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;

/**
 * Service for creating and manipulating User Model Views
 *
 * @author João
 */
public class UserMVService {

    /**
     * Returns a model view class based on the create user type
     *
     * @param type String with the authentication type
     * @return Class with the respective authentication model view class
     * @throws IllegalArgumentException Throws IllegalArgumentException if the
     * authentication type is invalid
     */
    public static Class<?> classFromType(String type) {
        if (type == null || type.isEmpty()) {
            throw new IllegalArgumentException("Invalid authentication type!");
        }
        switch (type) {
            case CredentialsAuthenticationMV.TYPE:
                return CreateCredentialsUserMV.class;
            default:
                throw new IllegalArgumentException("Invalid authentication type!");
        }
    }

    /**
     * Creates an instance of CreatedUserMV from an instance of Auth
     *
     * @param auth instance of Auth to aid in the creation
     * @return instance of CreatedUserMV
     */
    public static CreatedUserMV createdUserMVFromAuth(Auth auth) {
        CreatedUserMV createdUserMV = new CreatedUserMV();
        createdUserMV.token = auth.id();
        return createdUserMV;
    }
}
