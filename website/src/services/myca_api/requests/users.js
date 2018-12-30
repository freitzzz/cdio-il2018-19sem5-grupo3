//@ts-check
import { MYCA_API_URL } from "./../../../config";
import Axios, { AxiosPromise } from "axios";


export default {

    /**
     * Registers a User.
     * @param {*} user - User being registered.
     * @returns {AxiosPromise<any>} Axios Promise representing the registered user's token.
     */
    register(user) {
        return Axios.post(`${MYCA_API_URL}/users`, user);
    }
}