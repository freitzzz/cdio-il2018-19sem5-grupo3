
/**
 * Requires cookie services
 */
import {getMYCASessionToken} from './CookieService';

/**
 * Requires authorization services
 */
import {grantUserIsClient} from './AuthorizationService';

/**
 * Grants that a user is currently authenticated in MYC APIs
 */
export const grantUserIsAuthenticated=()=>{
    return new Promise((accept,reject)=>{
        let mycaSessionToken=getMYCASessionToken();
        if(!mycaSessionToken || mycaSessionToken.length==0)
            reject("User has no MYCA Cookie set");
        grantUserIsClient()
            .then(()=>{accept();})
            .catch(()=>{reject();});
    });
};