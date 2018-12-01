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
