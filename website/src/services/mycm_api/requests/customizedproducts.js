//@ts-check
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const CUSTOMIZED_PRODUCTS_URL = `${MYCM_API_URL}/customizedproducts`;


export default {

    /**
     * Retrieves all of the available CustomizedProducts.
     * @returns {AxiosPromise<any>} Axios Promise representing the available CustomizedProducts.
     */
    getCustomizedProducts() {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}`);
    },


    /**
     * Retrieves all of the available base CustomizedProducts.
     */
    getBaseCustomizedProducts() {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/base`);
    },


    /**
     * Retrieves a CustomizedProduct with a matching identifier.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     */
    getCustomizedProduct(customizedProductId) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}`);
    },


    /**
     * Retrieves a CustomizedProduct's recommended slot layout.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's recommended slot layout.
     */
    getCustomizedProductRecommendedSlots(customizedProductId) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/recommendedslots`);
    },


    /**
     * Retrieves a CustomizedProduct's minimum slot layout.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's minimum slot layout.
     */
    getCustomizedProductMinimumSlots(customizedProductId) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/minimumslots`);
    },

    /**
     * Retrieves a CustomizedProduct's Slot.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @param {Number} slotId - Slot's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's Slot.
     */
    getCustomizedProductSlot(customizedProductId, slotId) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots/${slotId}`);
    },

    /**
     * Retrieves a CustomizedProduct's price.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @param {String} currency - Currency in which the price will be displayed.
     * @param {String} area - CustomizedProduct's surface area.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's price.
     */
    getCustomizedProductPrice(customizedProductId, currency, area) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/price?currency=${currency}&area=${area}`);
    },

    /**
     * Options used for adding a CustomizedProduct to another CustomizedProduct's slot.
     * @typedef {Object} PostOptions
     * @property {Number} customizedProductId - CustomizedProduct's identifier.
     * @property {Number} slotId - Slot's identifier.
     */

    /**
     * Adds a new CustomizedProduct
     * @param {*} customizedProduct - CustomizedProduct being added.
     * @param {PostOptions=} postOptions - Additional options for posting a CustomizedProduct.
     * @returns {AxiosPromise<any>} Axios Promise representing the added CustomizedProduct.
     */
    postCustomizedProduct(customizedProduct, postOptions) {

        if (postOptions == undefined) {
            return Axios.post(`${CUSTOMIZED_PRODUCTS_URL}`, customizedProduct);
        } else {

            const { customizedProductId, slotId } = postOptions;

            return Axios.post(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots/${slotId}/customizedproducts`, customizedProduct);
        }
    },

    /**
     * Adds a new Slot to a CustomizedProduct.
     * @param {Number} customizedProductId 
     * @param {*} slot - Slot being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Slot.
     */
    postCustomizedProductSlot(customizedProductId, slot) {
        return Axios.post(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots`, slot);
    },

    /**
     * Updates a CustomizedProduct.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @param {*} customizedProduct - CustomizedProduct's updated data.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated CustomizedProduct. 
     */
    putCustomizedProduct(customizedProductId, customizedProduct) {
        return Axios.put(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}`, customizedProduct);
    },

    /**
     * Updates a CustomizedProduct's Slot.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @param {Number} slotId - Slot's identifier.
     * @param {*} slot - Slot's updated data.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated Slot.
     */
    putCustomizedProductSlot(customizedProductId, slotId, slot) {
        return Axios.put(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots/${slotId}`, slot);
    },

    /**
     * Deletes a CustomizedProduct.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted CustomizedProduct.
     */
    deleteCustomizedProduct(customizedProductId) {
        return Axios.delete(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}`);
    },

    /**
     * Deletes a CustomizedProduct's Slot.
     * @param {Number} customizedProductId - CustomizedProduct's identifier.
     * @param {Number} slotId - Slot's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Slot. 
     */
    deleteCustomizedProductSlot(customizedProductId, slotId) {
        return Axios.delete(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots/${slotId}`);
    }

}