import AxiosDependency, { AxiosPromise } from "axios";
import { MYCO_API_URL } from "./../../../config";

let Axios=AxiosDependency.create();

const CITIES_URL = `${MYCO_API_URL}/cities`;

export default {
    /**
     * Retrieves all of the existent cities.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the existent orders.
     */
    getCities() {
        return Axios.get(CITIES_URL);
    },

    /**
     * Activates a user account
     * @param {*} city info
     * @returns {AxiosPromise<any>} Axios Promise representing the activation of a user's account
     */
    createCity(cityInfo){
        return Axios.post(CITIES_URL, cityInfo);
    }
}