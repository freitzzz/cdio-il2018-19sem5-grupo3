package cdiomyc.core.mv.authorization;

import cdiomyc.core.mv.authentication.AuthenticationMV;
import java.time.LocalDateTime;

/**
 * Model View representation for the is user authorized operation model view
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class IsUserAuthorizedMV extends AuthenticationMV{
    /**
     * String with the user session API token
     */
    public String sessionAPIToken;
    /**
     * Boolean true if the user is a content manager
     */
    public boolean isContentManager;
}
