package cdiomyc.core.mv.authentication.session;

import cdiomyc.core.domain.auth.Session;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * Test class of AuthenticationSessionMVService
 *
 * @author João
 */
public class AuthenticationSessionMVServiceTest {

    /**
     * Ensure fromEntity works properly
     */
    @Test
    public void ensureFromEntityWorks() {
        LocalDateTime time = LocalDateTime.now().plusMinutes(60);
        String token = "Aria of the Starlight";
        Session authenticationSession = new Session(time, token,"secretetoken");
        GetAuthenticationSessionDetailsMV result = AuthenticationSessionMVService.fromEntity(authenticationSession);
        assertEquals(authenticationSession.tokenAsJWT(), result.token);
        assertEquals(time.format(DateTimeFormatter.ISO_DATE_TIME), result.sessionEnd);
    }

}
