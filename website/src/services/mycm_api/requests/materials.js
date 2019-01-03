//@ts-check
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const MATERIALS_URL = `${MYCM_API_URL}/materials`;


export default {

    /**
     * Retrieves all of the available materials.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available materials.
     */
    getMaterials() {
        return Axios.get(`${MATERIALS_URL}`);
    },


    /**
     * Retrieves a Material with a matching identifier.
     * @param {number} materialId - Material's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Material.
     */
    getMaterial(materialId) {
        return Axios.get(`${MATERIALS_URL}/${materialId}`);
    },


    /**
     * Adds a new Material.
     * @param {*} material - Material being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Material.
     */
    postMaterial(material) {
        return Axios.post(`${MATERIALS_URL}`, material);
    },

    /**
     * Adds a new Finish to the Material.
     * @param {number} materialId - Material's identifier.
     * @param {*} finish - Finish being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Finish.
     */
    postMaterialFinish(materialId, finish) {
        return Axios.post(`${MATERIALS_URL}/${materialId}/finishes`, finish);
    },

    /**
     * Adds a new Color to the Material.
     * @param {number} materialId - Material's identifier.
     * @param {*} color - Color being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Color.
     */
    postMaterialColor(materialId, color) {
        return Axios.post(`${MATERIALS_URL}/${materialId}/colors`, color);
    },

    /**
     * Updates a Material.
     * @param {number} materialId - Material's identifier.
     * @param {*} material - Material's data being updated.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated Material.
     */
    putMaterial(materialId, material) {
        return Axios.put(`${MATERIALS_URL}/${materialId}`, material);
    },

    /**
     * Deletes a Material.
     * @param {number} materialId - Material's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Material.
     */
    deleteMaterial(materialId) {
        return Axios.delete(`${MATERIALS_URL}/${materialId}`);
    },

    /**
     * Deletes a Material's Finish.
     * @param {number} materialId - Material's identifier.
     * @param {number} finishId - Finish's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Finish.
     */
    deleteMaterialFinish(materialId, finishId) {
        return Axios.delete(`${MATERIALS_URL}/${materialId}/finishes/${finishId}`);
    },

    /**
     * Deletes a Material's Color.
     * @param {number} materialId - Material's identifier.
     * @param {number} colorId - Color's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Color.
     */
    deleteMaterialColor(materialId, colorId) {
        return Axios.delete(`${MATERIALS_URL}/${materialId}/colors/${colorId}`);
    }

}