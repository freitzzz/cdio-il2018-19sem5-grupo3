package cdiomyc.core.domain.auth;

import cdiomyc.core.domain.auth.credentials.CredentialsAuth;
import cdiomyc.core.mv.authentication.AuthenticationMV;
import cdiomyc.core.mv.authentication.CredentialsAuthenticationMV;

/**
 *
 *
 * Factory service for creating Auth entitis based on authentication details
 *
 * @author João & Freitas
 */
public class AuthFactory {

    /**
     * Creates an Auth from certain authentication details
     *
     * @param authDetails AuthenticationMV with the authentication detials
     * @return Auth with the creating Auth
     * @throws IllegalStateException If authentication details are invalid
     */
    public static Auth createAuth(AuthenticationMV authDetails) {
        if (authDetails instanceof CredentialsAuthenticationMV) {
            CredentialsAuthenticationMV credentialDetails = (CredentialsAuthenticationMV) authDetails;
            return new CredentialsAuth(credentialDetails.username, credentialDetails.password);
        }
        throw new IllegalStateException("Invalid authentication details!");
    }

    /**
     * Validates a certain authentication details
     *
     * @param authDetails AuthenticationMV with the authentication details
     * @return Auth if the authentication details are valid
     * @throws IllegalStateException If authentication details are invalid
     */
    public static Auth validateAuth(AuthenticationMV authDetails) {
        try {
            return createAuth(authDetails);
        } catch (IllegalArgumentException invalidAuthCreation) {
            throw new IllegalStateException("Invalid authentication details!");
        }
    }
}
