package cdiomyc.core.application;

/**
 * Class that holds all global application context settings & dependencies
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class Application {
    /**
     * ApplicationSettings with the current application settings
     */
    private static final ApplicationSettings APPLICATION_SETTINGS=new ApplicationSettings();
    /**
     * Returns the current application settings
     * @return ApplicationSettings with the current application settings
     */
    public static ApplicationSettings settings(){return APPLICATION_SETTINGS;}
}
