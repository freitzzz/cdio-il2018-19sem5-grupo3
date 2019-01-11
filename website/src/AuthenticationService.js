
/**
 * Requires cookie services
 */
import {getMYCASessionToken} from './CookieService';

/**
 * Requires authorization services
 */
import {grantUserIsClient} from './AuthorizationService';

/**
 * Requires MYCA API URL for authentication requests
 */
import {MYCA_API_URL} from './config';

/**
 * Requires Axios for HTTP request
 */
import AxiosDependecy from 'axios';

/**
 * Private axios instance for users services
 */
let axios=AxiosDependecy.create();
axios.defaults.withCredentials=true;

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

/**
 * Authenticates an user
 * @param {Object} authenticationDetails Object with the user authentication details
 */
export const authenticateUser=(authenticationDetails)=>{
    return new Promise((accept,reject)=>{
        axios
            .post(MYCA_API_URL+"/auth",authenticationDetails)
            .then((authenticationData)=>{
                accept(authenticationData);
            })
            .catch((error_message)=>{
                reject(error_message.response.data);
            });
    });
};