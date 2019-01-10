/**
 * Requires cookie services
 */
import {getMYCASessionToken} from './CookieService';

/**
 * Requires getUserAuthorizations for identifying user authorizations
 */
import {getUserAuthorizations} from './AuthorizationService';

/**
 * Requires MYCA API URL for users requests
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
 * Returns the current authenticated user details
 */
export const getUserDetails=()=>{
    return new Promise((accept,reject)=>{
        let mycaSessionToken=getMYCASessionToken();
        if(!mycaSessionToken || mycaSessionToken.length==0)
            reject("User has no MYCA Cookie set");
        let userDetailsWithAuthorizations={};
        axios
            .get(MYCA_API_URL+"/users")
            .then((userDetails)=>{
                let userDetailsData=userDetails.data;
                userDetailsWithAuthorizations.name=userDetailsData.name;
                userDetailsWithAuthorizations.apiToken=userDetailsData.apiToken;
                getUserAuthorizations()
                    .then((userAuthorizations)=>{
                        userDetailsWithAuthorizations.roles={
                            isAdministrator:userAuthorizations.administrator,
                            isClient:userAuthorizations.client,
                            isContentManager:userAuthorizations.contentManager,
                            isLogisticManager:userAuthorizations.logisticManager
                        };
                        accept(userDetailsWithAuthorizations);
                    })
                    .catch((userAuthorizations)=>{
                        userDetailsWithAuthorizations.roles={
                            isAdministrator:userAuthorizations.administrator,
                            isClient:userAuthorizations.client,
                            isContentManager:userAuthorizations.contentManager,
                            isLogisticManager:userAuthorizations.logisticManager
                        };
                        accept(userDetailsWithAuthorizations);
                    });
            })
            .catch((error_message)=>{
                reject(error_message.response.data.message);
            });
    });
};