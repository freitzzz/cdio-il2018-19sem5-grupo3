
/**
 * Requires Axios for HTTP requests
 */
import Axios from 'axios';

/**
 * Requires MYC APIs URLs
 */
import {MYCA_API_URL} from './config';

/**
 * Grants that the authentication API is available
 */
const grantAuthenticationAPIIsAvailable=()=>{
    return checkConnectionAvailability(MYCA_API_URL+'/users');
};

/**
 * Checks if a connection is reached and available
 * @param {String} url String with the connection URL
 */
let checkConnectionAvailability=(url)=>{
    return Axios.request({url:url,method:'OPTIONS'});
};

/**
 * Exports MYC APIs grants service
 */
module.exports={
    grantAuthenticationAPIIsAvailable:grantAuthenticationAPIIsAvailable
};