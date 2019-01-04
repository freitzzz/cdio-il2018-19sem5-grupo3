//@ts-check
// eslint-disable-next-line no-unused-vars
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const PRICE_TABLES_URL = `${MYCM_API_URL}/prices`;

export default {

    /**
     * Retrieves price histories of all materials
     * @param {string} currency 
     * @param {string} area 
     * @returns {AxiosPromise<any>} Axios Promise representing the price histories of all materials
     */
    getPriceHistoryOfAllMaterials(currency, area) {
        return Axios.get(`${PRICE_TABLES_URL}/materials/?currency=${currency}&area=${area}`);
    },

    /**
     * Retrieves price history of a material
     * @param {number} materialId
     * @param {string} currency
     * @param {string} area
     * @returns {AxiosPromise<any>} Axios Promise representing the price history of a material
     */
    getMaterialPriceHistory(materialId, currency, area) {
        return Axios.get(`${PRICE_TABLES_URL}/materials/${materialId}/?currency=${currency}&area=${area}`);
    },

    /**
     * Retrieves price histories of all material finishes
     * @param {number} materialId 
     * @param {string} currency 
     * @param {string} area
     * @returns {AxiosPromise<any>} Axios Promise representing the price histories of all material finishes 
     */
    getPriceHistoryOfAllMaterialFinishes(materialId, currency, area) {
        return Axios.get(`${PRICE_TABLES_URL}/materials/${materialId}/finishes/?currency=${currency}&area=${area}`);
    },

    /**
     * Retrieves price history of a material finish
     * @param {number} materialId 
     * @param {number} finishId 
     * @param {string} currency 
     * @param {string} area
     * @returns {AxiosPromise<any>} Axios Promise representing the price history of a material finish 
     */
    getMaterialFinishPriceHistory(materialId, finishId, currency, area) {
        return Axios.get(`${PRICE_TABLES_URL}/materials/${materialId}/finishes/${finishId}/?currency=${currency}&area=${area}`);
    },

    /**
     * Retrieves the current price of a material
     * @param {number} materialId 
     * @param {string} currency 
     * @param {string} area 
     * @returns {AxiosPromise<any>} Axios Promise representing the current price of a material
     */
    getCurrentMaterialPrice(materialId, currency, area) {
        return Axios.get(`${PRICE_TABLES_URL}/materials/${materialId}/currentprice?currency=${currency}&area=${area}`);
    },

    /**
     * Retrieves the current price of a material finish
     * @param {number} materialId 
     * @param {number} finishId 
     * @param {string} currency 
     * @param {string} area
     * @returns {AxiosPromise<any>} Axios Promise representing the current price of a material finish} 
     */
    getCurrentMaterialFinishPrice(materialId, finishId, currency, area) {
        return Axios.get(`${PRICE_TABLES_URL}/materials/${materialId}/finishes/${finishId}/currentprice?currency=${currency}&area=${area}`);
    },

    /**
     * Adds a new material price table entry
     * @param {number} materialId 
     * @param {*} materialPriceTableEntry 
     * @returns {AxiosPromise<any>} Axios Promise representing the added entry
     */
    postMaterialPriceTableEntry(materialId, materialPriceTableEntry) {
        return Axios.post(`${PRICE_TABLES_URL}/materials/${materialId}`, materialPriceTableEntry);
    },

    /**
     * Adds a new material finish price table entry
     * @param {number} materialId 
     * @param {number} finishId 
     * @param {*} finishPriceTableEntry
     * @returns {AxiosPromise<any>} Axios Promise representing the added entry 
     */
    postMaterialFinishPriceTableEntry(materialId, finishId, finishPriceTableEntry) {
        return Axios.post(`${PRICE_TABLES_URL}/materials/${materialId}/finishes/${finishId}`, finishPriceTableEntry);
    },

    /**
     * Updates a material price table entry
     * @param {number} materialId 
     * @param {number} entryId 
     * @param {*} updatedEntry
     * @returns {AxiosPromise<any>} Axios Promise representing the updated entry 
     */
    putMaterialPriceTableEntry(materialId, entryId, updatedEntry) {
        return Axios.put(`${PRICE_TABLES_URL}/materials/${materialId}/entries/${entryId}`, updatedEntry);
    },

    /**
     * Updates a material finish price table entry
     * @param {number} materialId 
     * @param {number} finishId 
     * @param {number} entryId 
     * @param {*} updatedEntry 
     */
    putMaterialFinishPriceTableEntry(materialId, finishId, entryId, updatedEntry) {
        return Axios.put(`${PRICE_TABLES_URL}/materials/${materialId}/finishes/${finishId}/entries/${entryId}`, updatedEntry);
    }
}