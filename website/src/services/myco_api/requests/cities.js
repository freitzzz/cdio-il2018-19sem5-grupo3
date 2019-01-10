import Axios, { AxiosPromise } from "axios";
import { MYCO_API_URL } from "./../../../config";

const CITIES_URL = `${MYCO_API_URL}/cities`;

export default {
    /**
     * Retrieves all of the existent cities.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the existent orders.
     */
    getCities() {
        return Axios.get(`${CITIES_URL}`);
    }
}