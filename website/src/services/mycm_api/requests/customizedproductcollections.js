import Axios, { AxiosPromise } from 'axios'
import { MYCM_API_URL } from '../../../config'

export default {

    /**
     * Retrieves all of the available CustomizedProductCollections.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available CustomizedProductCollections.
     */
    getCustomizedProductCollections() {
        return Axios.get(`${MYCM_API_URL}/collections`);
    },

    /**
     * Retrieves a CustomizedProductCollection by its identifier.
     * @param {number} collectionId - CustomizedProductCollection's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing he CustomizedProductCollection.
     */
    getCustomizedProductCollection(collectionId) {
        return Axios.get(`${MYCM_API_URL}/collections/${collectionId}`)
    },

    /**
     * Retrieves a CustomizedProductCollection by its name.
     * @param {string} collectionName - CustomizedProductCollection's name.
     * @returns {AxiosPromise<any>} Axios Promise representing the CustomizedProductCollection.  
     */
    getCustomizedProductCollectionByName(collectionName) {
        return Axios.get(`${MYCM_API_URL}/collections/?name=${collectionName}`)
    },

    /**
     * Adds a CustomizedProductCollection.
     * @param {*} collection 
     * @returns {AxiosPromise<any>} Axios Promise representing the added CustomizedProductCollection.
     */
    postCustomizedProductCollection(collection) {
        return Axios.post(`${MYCM_API_URL}/collections`, collection);
    },

    /**
     * Adds a CustomizedProduct to a CustomizedProductCollection.
     * @param {number} collectionId - CustomizedProductCollection's persistence identifier.
     * @param {*} customizedProduct 
     */
    postCustomizedProductCollectionCustomizedProduct(collectionId, customizedProduct) {
        return Axios.post(`${MYCM_API_URL}/collections/${collectionId}/customizedproducts`, customizedProduct);
    },

    /**
     * Updates a CustomizedProductCollection.
     * @param {number} collectionId - CustomizedProductCollection's persistence identifier.
     * @param {*} collection - CustomizedProductCollection's updated data.
     */
    putCustomizedProductCollection(collectionId, collection) {
        return Axios.put(`${MYCM_API_URL}/collections/${collectionId}`, collection);
    },

    /**
     * Deletes a CustomizedProductCollection.
     * @param {number} collectionId - CustomizedProductCollection's identifier.
     */
    deleteCustomizedProductCollection(collectionId) {
        return Axios.delete(`${MYCM_API_URL}/collections/${collectionId}`);
    },

    /**
     * Deletes a CustomizedProduct from a CustomizedProductCollection.
     * @param {number} collectionId - CustomizedProductCollection's identifier.
     * @param {number} customizedProductId - CustomizedProduct's identifier.
     */
    deleteCustomizedProductCollectionCustomizedProduct(collectionId, customizedProductId) {
        return Axios.delete(`${MYCM_API_URL}/collections/${collectionId}/customizedproducts/${customizedProductId}`);
    }
}