//@ts-check
import { MYCA_API_URL } from "./../../../config";
import Axios, { AxiosPromise } from "axios";


export default {

    /**
     * Options used for authorization.
     * @typedef {Object} AuthorizationOptions
     * @property {Boolean} contentManager - used for checking if the User is authorized as a content manager
     */


    /**
     * Checks if the user is authorized.
     * @param {AuthorizationOptions=} authoOptions - Options used for authorization.
     */
    checkAuthorization(authoOptions) {

        //TODO: set cookies

        if (authoOptions == undefined) {
            return Axios.get(`${MYCA_API_URL}/autho`, {});
        } else {
            return Axios.get(`${MYCA_API_URL}/autho/?contentManager=${authoOptions.contentManager}`, {});
        }
    }

}