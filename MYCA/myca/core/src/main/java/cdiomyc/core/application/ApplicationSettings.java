package cdiomyc.core.application;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 * Class that represents the current application settings
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class ApplicationSettings {
    /**
     * Constant that represents the name of injector which will inject the 
     * current application settings
     */
    private static final String SETTINGS_INJECTOR="application-settings";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current application persistence context
     */
    private static final String PERSISTENCE_CONTEXT_KEY="persistence.context";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current username digest salt being used
     */
    private static final String USERNAME_DIGEST_SALT_KEY="username.digest.salt";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current password digest salt being used
     */
    private static final String PASSWORD_DIGEST_SALT_KEY="password.digest.salt";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current username digest algorithm being used
     */
    private static final String USERNAME_DIGEST_ALGORITHM_KEY="username.digest.algorithm";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current password digest algorithm being used
     */
    private static final String PASSWORD_DIGEST_ALGORITHM_KEY="password.digest.algorithm";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current username operators encryption algorithm being used
     */
    private static final String USERNAME_OPERATORS_ALGORITHM_KEY="username.operators.algorithm";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current username operators encryption algorithm being used
     */
    private static final String PASSWORD_OPERATORS_ALGORITHM_KEY="password.operators.algorithm";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current username operators encryption algorithm being used
     */
    private static final String USERNAME_OPERATORS_VALUE_KEY="username.operators.value";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current username operators encryption algorithm being used
     */
    private static final String PASSWORD_OPERATORS_VALUE_KEY="password.operators.value";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current authentication session secrete identifier digest algorithm
     */
    private static final String AUTHENTICATION_SESSION_SECRETE_IDENTIFIER_DIGEST_ALGORITHM_KEY="authentication.session.secrete.identifier.digest.algorithm";
    /**
     * Constant that represents the name of the key which holds the value for 
     * the current authentication session secrete identifier digest salt
     */
    private static final String AUTHENTICATION_SESSION_SECRETE_IDENTIFIER_DIGEST_SALT_KEY="authentication.session.secrete.identifier.digest.salt";
    /**
     * Properties with the holder of the current application settings
     */
    private final Properties applicationSettings;
    /**
     * Protected constructor which builds a new ApplcationSettings the 
     */
    //Constructor is protected in order to allow settings to be refreshed by the 
    //application caller
    protected ApplicationSettings(){applicationSettings=injectSettings();}
    /**
     * Method that gets the current application persistence context
     * @return String with the current application persistence context
     */
    public String getPersistenceContext(){return applicationSettings.getProperty(PERSISTENCE_CONTEXT_KEY);}
    
    /**
     * Method that gets the current username digest salt being used
     * @return String with the current username digest salt being used
     */
    public String getUsernameSalt(){return applicationSettings.getProperty(USERNAME_DIGEST_SALT_KEY);}
    
    /**
     * Method that gets the current password digest salt being used
     * @return String with the current password digest salt being used
     */
    public String getPasswordSalt(){return applicationSettings.getProperty(PASSWORD_DIGEST_SALT_KEY);}
    
    /**
     * Method that gets the current username digest algorithm being used
     * @return String with the current username digest algorithm being used
     */
    public String getUsernameAlgorithm(){return applicationSettings.getProperty(USERNAME_DIGEST_ALGORITHM_KEY);}
    
    /**
     * Method that gets the current password digest algorithm being used
     * @return String with the current password digest algorithm being used
     */
    public String getPasswordAlgorithm(){return applicationSettings.getProperty(PASSWORD_DIGEST_ALGORITHM_KEY);}
    
    /**
     * Method that gets the current username operators encryption algorithm being used
     * @return String with the current username operators encryption algorithm being used
     */
    public String getUsernameOperatorsEncryptionAlgorithm(){return applicationSettings.getProperty(USERNAME_OPERATORS_ALGORITHM_KEY);}
    
    /**
     * Method that gets the current password operators encryption algorithm being used
     * @return String with the current password operators encryption algorithm being used
     */
    public String getPasswordOperatorsEncryptionAlgorithm(){return applicationSettings.getProperty(PASSWORD_OPERATORS_ALGORITHM_KEY);}
    
    /**
     * Method that gets the current username operators encryption value being used
     * @return Integer with the current username operators encryption value being used
     */
    public int getUsernameOperatorsEncryptionValue(){return Integer.parseInt(applicationSettings.getProperty(USERNAME_OPERATORS_VALUE_KEY));}
    
    /**
     * Method that gets the current password operators encryption value being used
     * @return Integer with the current password operators encryption value being used
     */
    public int getPasswordOperatorsEncryptionValue(){return Integer.parseInt(applicationSettings.getProperty(PASSWORD_OPERATORS_VALUE_KEY));}
    
    /**
     * Method that gets the current authentication session secrete identifier digest algorithm
     * @return String with the current authentication session secrete identifier digest algorithm
     */
    public String getAuthenticationSessionSecreteIdentifierDigestAlgorithm(){return applicationSettings.getProperty(AUTHENTICATION_SESSION_SECRETE_IDENTIFIER_DIGEST_ALGORITHM_KEY);}
    
    /**
     * Method that gets the current authentication session secrete identifier digest salt
     * @return String with the current authentication session secrete identifier digest salt
     */
    public String getAuthenticationSessionSecreteIdentifierDigestSalt(){return applicationSettings.getProperty(AUTHENTICATION_SESSION_SECRETE_IDENTIFIER_DIGEST_SALT_KEY);}
    
    /**
     * Method that injects the current application settings
     * @return Properties with the holder of the current application settings
     */
    private Properties injectSettings(){
        try {
            return injectSettings(ApplicationSettings.class.getClassLoader()
                    .getResourceAsStream(SETTINGS_INJECTOR));
        } catch (IOException ioException) {
            throw new IllegalStateException(ioException);
        }
    }
    /**
     * Method that injects a certain settings from a certain input stream
     * @param settingsInputStream InputStream with the input stream where 
     * the settings are located
     * @return Properties with the holder of a certain settings
     * @throws Throws {@link IOException} if an error occures while injecting the 
     * settings
     */
    private Properties injectSettings(InputStream settingsInputStream)throws IOException{
        Properties injectedSettings=new Properties();
        injectedSettings.load(settingsInputStream);
        return injectedSettings;
    }
}
