//@ts-check
import { MYCA_API_URL } from "./../../../config";
// eslint-disable-next-line no-unused-vars
import Axios, { AxiosPromise } from "axios";


export default {

    /**
     * Registers a User.
     * @param {*} user - User being registered.
     * @returns {AxiosPromise<any>} Axios Promise representing the registered user's token.
     */
    register(user) {
        return Axios.post(`${MYCA_API_URL}/users`, user);
    },

    /**
     * Activates a user account
     * @param {*} activationInfo 
     * @returns {AxiosPromise<any>} Axios Promise representing the activation of a user's account
     */
    activateAccount(activationInfo){
        return Axios.post(`${MYCA_API_URL}/users/activate`, activationInfo);
    }
}