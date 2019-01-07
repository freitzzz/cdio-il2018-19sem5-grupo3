package cdiomyc.webservices.configuration;

/**
 * Class that holds all global websettings context settings & dependencies
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class WebservicesConfiguration {
    /**
     * WebservicesConfiguration with the current application settings
     */
    private static final WebservicesConfigurationSettings WEBSERVICES_SETTINGS=new WebservicesConfigurationSettings();
    /**
     * Returns the current webservices settings
     * @return WebservicesConfigurationSettings with the current application settings
     */
    public static WebservicesConfigurationSettings settings(){return WEBSERVICES_SETTINGS;}
}
