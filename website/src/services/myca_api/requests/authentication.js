//@ts-check
import { MYCA_API_URL } from "./../../../config";
import Axios, { AxiosPromise } from "axios";

export default {

    /**
     * Authenticates a user.
     * @param {*} user - User being authenticated.
     * @returns {AxiosPromise<any>} Axios Promise representing the authentication token.
     */
    authenticate(user) {
        return Axios.post(`${MYCA_API_URL}/auth`, user);
    }
}