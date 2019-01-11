//@ts-check
// eslint-disable-next-line no-unused-vars
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const CURRENCIES_PER_AREA_URL = `${MYCM_API_URL}/currenciesperarea`;

export default {

    /**
     * Retrieves all available currencies
     * @returns {AxiosPromise<any>} Axios Promise representing all available currencies
     */
    getCurrencies() {
        return Axios.get(`${CURRENCIES_PER_AREA_URL}/currencies`);
    },

    /**
     * Retrieves all available areas
     * @returns {AxiosPromise<any>} Axios Promise representing all available areas
     */
    getAreas() {
        return Axios.get(`${CURRENCIES_PER_AREA_URL}/areas`)
    },

    /**
     * Converts a given value in a currency per area to another currency per area
     * @param {string} fromCurrency
     * @param {string} toCurrency
     * @param {string} fromArea
     * @param {string} toArea
     * @param {number} value
     * @return {AxiosPromise<any>} Axios Promise representing the converted value
     */
    convertValue(fromCurrency, toCurrency, fromArea, toArea, value){
        return Axios.get(`${CURRENCIES_PER_AREA_URL}/convert?fromCurrency=${fromCurrency}&toCurrency=${toCurrency}&fromArea=${fromArea}&toArea=${toArea}&value=${value}`);
    }
}