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
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available base CustomizedProducts.
     */
    getBaseCustomizedProducts() {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/base`);
    },


    /**
     * Retrieves a CustomizedProduct with a matching identifier.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     */
    getCustomizedProduct(customizedProductId) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}`);
    },


    /**
     * Retrieves a CustomizedProduct's recommended slot layout.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's recommended slot layout.
     */
    getCustomizedProductRecommendedSlots(customizedProductId) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/recommendedslots`);
    },


    /**
     * Retrieves a CustomizedProduct's minimum slot layout.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's minimum slot layout.
     */
    getCustomizedProductMinimumSlots(customizedProductId, unit) {

        var requestURL = `${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/minimumslots`;

        if (unit !== undefined) {
            requestURL = requestURL.concat(`?unit=${unit}`);
        }

        return Axios.get(requestURL);
    },

    /**
     * Retrieves a CustomizedProduct's Slot.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @param {number} slotId - Slot's identifier.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's Slot.
     */
    getCustomizedProductSlot(customizedProductId, slotId, unit) {

        var requestURL = `${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots/${slotId}`;

        if (unit !== undefined) {
            requestURL = requestURL.concat(`?unit=${unit}`);
        }

        return Axios.get(requestURL);
    },

    /**
     * Retrieves a CustomizedProduct's price.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @param {string} currency - Currency in which the price will be displayed.
     * @param {string} area - CustomizedProduct's surface area.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProduct's price.
     */
    getCustomizedProductPrice(customizedProductId, currency, area) {
        return Axios.get(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/price?currency=${currency}&area=${area}`);
    },

    /**
     * Options used for adding a CustomizedProduct to another CustomizedProduct's slot.
     * @typedef {object} PostOptions
     * @property {number} customizedProductId - CustomizedProduct's identifier.
     * @property {number} slotId - Slot's identifier.
     */

    /**
     * Adds a new CustomizedProduct
     * @param {*} customizedProduct - CustomizedProduct being added.
     * @param {PostOptions=} postOptions - Additional options for posting a CustomizedProduct.
     * @returns {AxiosPromise<any>} Axios Promise representing the added CustomizedProduct.
     */
    postCustomizedProduct(customizedProduct, postOptions) {

        var requestUrl = `${CUSTOMIZED_PRODUCTS_URL}`;

        if (postOptions !== undefined) {

            const { customizedProductId, slotId } = postOptions;

            requestUrl = requestUrl.concat(`/${customizedProductId}/slots/${slotId}/customizedproducts`);
        }

        return Axios.post(requestUrl, customizedProduct);
    },

    /**
     * Adds a new Slot to a CustomizedProduct.
     * @param {number} customizedProductId 
     * @param {*} slot - Slot being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Slot.
     */
    postCustomizedProductSlot(customizedProductId, slot) {
        return Axios.post(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots`, slot);
    },

    /**
     * Adds the recommended Slot layout to a CustomizedProduct.
     * @param {*} customizedProductId - CustomizedProduct's identifier.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit
     * @returns {AxiosPromise<any>} Axios Promise representing the updated CustomizedProduct with the recommended slots.
     */
    postCustomizedProductRecommendedSlots(customizedProductId, unit) {

        var requestURL = `${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/recommendedslots`;

        if (unit !== undefined) {
            requestURL = requestURL.concat(`?unit=${unit}`);
        }

        return Axios.post(requestURL);
    },

    /**
     * Adds the minimum Slot layout to a CustomizedProduct.
     * @param {*} customizedProductId - CustomizedProduct's identifier.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit
     * @returns {AxiosPromise<any>} Axios Promise representing the updated CustomizedProduct with minimum slots.
     */
    postCustomizedProductMinimumSlots(customizedProductId, unit) {

        var requestURL = `${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/minimumslots`;

        if (unit !== undefined) {
            requestURL = requestURL.concat(`?unit=${unit}`);
        }

        return Axios.post(requestURL);
    },

    /**
     * Updates a CustomizedProduct.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @param {*} customizedProduct - CustomizedProduct's updated data.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated CustomizedProduct. 
     */
    putCustomizedProduct(customizedProductId, customizedProduct) {
        return Axios.put(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}`, customizedProduct);
    },

    /**
     * Updates a CustomizedProduct's Slot.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @param {number} slotId - Slot's identifier.
     * @param {*} slot - Slot's updated data.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated Slot.
     */
    putCustomizedProductSlot(customizedProductId, slotId, slot) {
        return Axios.put(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots/${slotId}`, slot);
    },

    /**
     * Deletes a CustomizedProduct.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted CustomizedProduct.
     */
    deleteCustomizedProduct(customizedProductId) {
        return Axios.delete(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}`);
    },

    /**
     * Deletes a CustomizedProduct's Slot.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     * @param {number} slotId - Slot's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Slot. 
     */
    deleteCustomizedProductSlot(customizedProductId, slotId) {
        return Axios.delete(`${CUSTOMIZED_PRODUCTS_URL}/${customizedProductId}/slots/${slotId}`);
    }

}