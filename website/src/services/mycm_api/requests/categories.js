//@ts-check
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const CATEGORIES_URL = `${MYCM_API_URL}/categories`;

export default {

    /**
     * Retrieves all of the available Categories.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available Categories.
     */
    getCategories() {
        return Axios.get(`${CATEGORIES_URL}`);
    },

    /**
     * Retrieves all of a Category's sub Categories.
     * @param {*} categoryId - Category's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available sub Categories of the Category with a matching identifier. 
     */
    getSubCategories(categoryId) {
        return Axios.get(`${CATEGORIES_URL}/${categoryId}/subcategories`);
    },


    /**
     * Retrieves a Category by its name.
     * @param {*} categoryName - Name of the Category.
     * @returns {AxiosPromise<any>} Axios Promise representing the Category with the given name.
     */
    getCategoryByName(categoryName) {
        return Axios.get(`${CATEGORIES_URL}/?name=${categoryName}`);
    },

    /**
     * Retrieves the Category with a matching identifier.
     * @param {Number} categoryId- Category's identifier.
     * @param {Boolean} withParent - Boolean representing whether or not Parent Category information should also be retrieved.
     * @returns {AxiosPromise<any>} Axios Promise representing the Category with a matching identifier. 
     */
    getCategoryById(categoryId, withParent) {
        return Axios.get(`${CATEGORIES_URL}/${categoryId}/?withparent=${withParent}`);
    },

    /**
     * Adds a new Category.
     * @param {*} category - Category being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Category.  
     */
    postCategory(category) {
        return Axios.post(`${CATEGORIES_URL}`, category);
    },

    /**
     * Adds a new sub Category to the Category with a matching identifier.
     * @param {Number} parentCategoryId - Parent Category's identifier.
     * @param {*} category - Category being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Category.
     */
    postSubCategory(parentCategoryId, category) {
        return Axios.post(`${CATEGORIES_URL}/${parentCategoryId}/subcategories`, category);
    },

    /**
     * Updates a Category's data.
     * @param {Number} categoryId - Category's identifier.
     * @param {*} category - Category's new data.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated Category.
     */
    putCategory(categoryId, category) {
        return Axios.put(`${CATEGORIES_URL}/${categoryId}`, category);
    },

    /**
     * Deletes a Category.
     * @param {Number} categoryId - Category's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Category.
     */
    deleteCategory(categoryId) {
        return Axios.delete(`${CATEGORIES_URL}/${categoryId}`);
    }

}