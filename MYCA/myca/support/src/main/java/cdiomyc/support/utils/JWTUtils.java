package cdiomyc.support.utils;

import com.auth0.jwt.JWT;
import com.auth0.jwt.algorithms.Algorithm;
import com.auth0.jwt.exceptions.JWTVerificationException;

/**
 * Simple utility class to encode and decode data in JWT
 * @author <a href="https://github.com/freitzzz">freitzzz</a>
 */
public final class JWTUtils {
    /**
     * Constant that represents the default verify signature
     */
    private static final String DEFAULT_SECRETE_SIGNATURE="kerokerobonito";
    /**
     * Encodes a piece of data with the following JWT options:
     * <br>
     * <ul>
     *      <li><b>Header</b>
     *          <ul>
     *              <li><b>alg</b>:HS256</li>
     *              <li><b>typ</b>:JWT</li>
     *          </ul>
     *      </li>
     *      <li><b>Payload</b>
     *          <ul>
     *              <li><b>data</b>:$DATA_HERE</li>
     *          </ul>
     *      </li>
     *      <li><b>Verify Signature</b>
     *          <ul>
     *              <li><b>secrete</b>:$SECRETE_SIGNATURE_HERE</li>
     *          </ul>
     *      </li>
     * </ul>
     * @param data String with the data being encoded
     * @return String with the data encoded as JWT
     */
    public static String encode(String data){
        return JWT
                .create()
                .withClaim("data",data)
                .sign(Algorithm.HMAC256(DEFAULT_SECRETE_SIGNATURE));
    }
    
    /**
     * Verifies if a JWT is valid with default JWT options
     * <br>
     * <ul>
     *      <li><b>Header</b>
     *          <ul>
     *              <li><b>alg</b>:HS256</li>
     *              <li><b>typ</b>:JWT</li>
     *          </ul>
     *      </li>
     *      <li><b>Payload</b>
     *          <ul>
     *              <li><b>data</b>:$DATA_HERE</li>
     *          </ul>
     *      </li>
     *      <li><b>Verify Signature</b>
     *          <ul>
     *              <li><b>secrete</b>:$SECRETE_SIGNATURE_HERE</li>
     *          </ul>
     *      </li>
     * </ul>
     * @param jwtData String with the JWT data being decoded
     * @return String with the decoded data
     */
    public static String decode(String jwtData){
        try{
            return JWT
                .require(Algorithm.HMAC256(DEFAULT_SECRETE_SIGNATURE))
                .withIssuer("auth0")
                .build()
                .verify(jwtData)
                .getClaim("data")
                .asString();
        }catch(NullPointerException|JWTVerificationException | IllegalArgumentException invalidJWT){
            throw new IllegalStateException("The JWT is invalid");
        }
    }
}
