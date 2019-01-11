
/**
 * Requires Axios for HTTP requests
 */
import AxiosDependency from "axios";

/**
 * Axios instance used for submitting MYCA's requests.
 */
const Axios = AxiosDependency.create();

/**
 * Sets all Axios requests to include cookies
 */
Axios.defaults.withCredentials=true;


/**
 * Requires MYCA API url
 */
import {MYCA_API_URL} from "./config";

/**
 * Grants that a user is an administrator in a promise way
 */
export const grantUserIsAdministrator=()=>{
    return Axios.get(MYCA_API_URL+"/autho?contentmanager=true");
};

/**
 * Grants that a user is a client in a promise way
 */
export const grantUserIsClient=()=>{
    return Axios.get(MYCA_API_URL+"/autho");
};

/**
 * Grants that a user is a content manager in a promise way
 */
export const grantUserIsContentManager=()=>{
    return Axios.get(MYCA_API_URL+"/autho?contentmanager=true");
};

/**
 * Grants that a user is a logistic manager in a promise way
 */
export const grantUserIsLogisticManager=()=>{
    return Axios.get(MYCA_API_URL+"/autho?logisticmanager=true");
};

/**
 * Returns all user authorizations in a object in a promise way
 */
export const getUserAuthorizations=()=>{
    return new Promise((accept,reject)=>{
        let userAuthorizations={
            administrator:false,
            client:false,
            contentManager:false,
            logisticManager:false
        };
        AxiosDependency
            .all([grantUserIsAdministrator(),grantUserIsClient(),grantUserIsContentManager(),grantUserIsLogisticManager()])
            .then(([isAdminstrator,isClient,isContentManager,isLogisticManager])=>{
                userAuthorizations.administrator=isAdminstrator.status==204;
                userAuthorizations.client=isClient.status==204;
                userAuthorizations.contentManager=isContentManager.status==204;
                userAuthorizations.logisticManager=isLogisticManager.status==204;
                accept(userAuthorizations);
            })
            .catch(()=>{
                //TODO: Investigate
                reject(userAuthorizations);
            });
    });
};