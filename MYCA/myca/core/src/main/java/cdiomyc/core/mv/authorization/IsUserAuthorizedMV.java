package cdiomyc.core.mv.authorization;

/**
 * Model View representation for the is user authorized operation model view
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class IsUserAuthorizedMV {
    /**
     * String with the user session API token
     */
    public String sessionAPIToken;
    /**
     * Boolean true if the user is a content manager
     */
    public boolean isContentManager;
}
