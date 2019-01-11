package cdiomyc.webservices.configuration;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 * Class that represents the current application settings
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class WebservicesConfigurationSettings {
    /**
     * Constant that represents the name of injector which will inject the 
     * current application settings
     */
    private static final String SETTINGS_INJECTOR="webservices-settings";
    
    /**
     * Constant that represents the name of the depedency that holds the current 
     * email services carrier name
     */
    private static final String CURRENT_EMAIL_CARRIER="CURRENT_EMAIL_CARRIER";
    
    /**
     * Constant that represents the name of the depedency that holds the current 
     * SMS services carrier name
     */
    private static final String CURRENT_SMS_CARRIER="CURRENT_SMS_CARRIER";
    
    
    /**
     * Constant that represents the name of the depedency that holds Nexmo API key 
     * to be used on SMS services using Nexmo as the 
     */
    private static final String NEXMO_API_KEY="NEXMO_API_KEY";
    
    /**
     * Constant that represents the name of the depedency that holds Nexmo API secret 
     * to be used on SMS services using Nexmo as the 
     */
    private static final String NEXMO_API_SECRET="NEXMO_API_SECRET";
    
    /**
     * Constant that represents the name of the depedency that holds SendGrid API key 
     * to be used on email services using SendGrid as the 
     */
    private static final String SEND_GRID_API_KEY="SEND_GRID_API_KEY";
    
    /**
     * Properties with the holder of the current application settings
     */
    private final Properties webservicesSettings;
    /**
     * Protected constructor which builds a new ApplcationSettings the 
     */
    //Constructor is protected in order to allow settings to be refreshed by the 
    //application caller
    protected WebservicesConfigurationSettings(){webservicesSettings=injectSettings();}
    
    /**
     * Returns the current email carrier to be used email services
     * @return String with the current email carrier
     */
    public String getCurrentEmailCarrier(){return (String)webservicesSettings.getProperty(CURRENT_EMAIL_CARRIER);}
    
    /**
     * Returns the current SMS carrier to be used SMS services
     * @return String with the current SMS carrier
     */
    public String getCurrentSMSCarrier(){return (String)webservicesSettings.getProperty(CURRENT_SMS_CARRIER);}
    
    /**
     * Returns the current Nexmo API key to be used on SMS services with Nexmo SMS carrying services
     * @return String with the current Nexmo API key
     */
    public String getNexmoAPIKey(){return (String)webservicesSettings.getProperty(NEXMO_API_KEY);}
    
    /**
     * Returns the current Nexmo API secret to be used on SMS services with Nexmo SMS carrying services
     * @return String with the current Nexmo API secret
     */
    public String getNexmoAPISecret(){return (String)webservicesSettings.getProperty(NEXMO_API_SECRET);}
    
    /**
     * Returns the current SendGrid API key to be used on email services with SendGrid email carrying services
     * @return String with the current SendGrid API key
     */
    public String getSendGridAPIKey(){return (String)webservicesSettings.getProperty(SEND_GRID_API_KEY);}
    
    /**
     * Method that injects the current application settings
     * @return Properties with the holder of the current application settings
     */
    private Properties injectSettings(){
        try {
            return injectSettings(WebservicesConfigurationSettings.class.getClassLoader()
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
