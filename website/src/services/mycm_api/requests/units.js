//@ts-check
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const UNITS_URL = `${MYCM_API_URL}/units`;

export default {

    /**
     * Retrieves all of the available dimensional units.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available units.
     */
    getUnits() {
        return Axios.get(`${UNITS_URL}`);
    },


    /**
     * Converts a given value in a unit to another unit.
     * @param {"mm" | "cm" | "dm" | "m"} fromUnit
     * @param {"mm" | "cm" | "dm" | "m"} toUnit 
     * @param {Number} value 
     * @return {AxiosPromise<any>} Axios Promise representing the converted value.
     */
    convertValue(fromUnit, toUnit, value) {
        return Axios.get(`${UNITS_URL}/?from=${fromUnit}&to=${toUnit}&value=${value}`);
    }

}