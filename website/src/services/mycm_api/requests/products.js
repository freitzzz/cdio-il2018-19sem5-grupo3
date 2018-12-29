//@ts-check
import Axios, { AxiosPromise } from "axios";
import { MYCM_API_URL } from "./../../../config";

const PRODUCTS_URL = `${MYCM_API_URL}/products`;


export default {

    /**
     * Retrieves all of the available Products.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available Products.
     */
    getProducts() {
        return Axios.get(`${PRODUCTS_URL}`);
    },


    /**
     * Retrieves all of the available base Products.
     * @returns {AxiosPromise<any>} Axios Promise representing all of the available base Products.
     */
    getBaseProducts() {
        return Axios.get(`${PRODUCTS_URL}/base`);
    },

    /**
     * Retrieves the Product with a matching identifier.
     * @param {Number} productId - Product's identifier.
     * @param {"mm" | "cm" | "dm" | "m"} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product. 
     */
    getProductById(productId, unit) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/?unit=${unit}`);
    },

    /**
     * Retrieves the Product with a matching reference.
     * @param {String} productReference - Product's reference.
     * @param {"mm" | "cm" | "dm" | "m"} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product.
     */
    getProductByReference(productReference, unit) {
        return Axios.get(`${PRODUCTS_URL}/?reference=${productReference}&unit=${unit}`)
    },

    /**
     * Retrieves the Product's dimensions.
     * @param {String} productId - Product's identifier.
     * @param {"mm" | "cm" | "dm" | "m"} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product's dimensions.
     */
    getProductDimensions(productId, unit) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/dimensions/?unit=${unit}`);
    },

    /**
     * Retrieves the Product's components.
     * @param {Number} productId - Product's identifier.
     * @param {"default" | "category"} groupOption - Option defining how the components will be grouped together.
     * @returns {AxiosPromise<any>} Axios Promise representing the Product's components.
     */
    getProductComponents(productId, groupOption) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/?groupby=${groupOption}`);
    },

    /**
     * Retrieves the Product's materials.
     * @param {Number} productId - Product's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product's materials.
     */
    getProductMaterials(productId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/materials`)
    },

    /**
     * Retrieves the Product's slot widths.
     * @param {*} productId - Product's identifier.
     * @param {"mm" | "cm" | "dm" | "m"} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product's slot widths.
     */
    getProductSlotWidths(productId, unit) {
        return Axios.get(`${PRODUCTS_URL}/slotwidths/?unit=${unit}`);
    },


    /**
     * Retrieves a Product's component.
     * @param {*} productId - Product's identifier.
     * @param {*} componentId - Component's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved component.
     */
    getProductComponent(productId, componentId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/components/${componentId}`);
    },

    /**
     * Retrieves the restrictions from a Product's dimension.
     * @param {Number} productId - Product identifier.
     * @param {Number} dimensionId - Dimension identifier. 
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved dimension's restrictions.
     */
    getProductDimensionRestrictions(productId, dimensionId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}/restrictions`);
    },

    /**
     * Retrieves the restrictions from a Product's component.
     * @param {Number} productId - Product's identifier.
     * @param {Number} componentId - Component's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the component's restrictions.
     */
    getProductComponentRestrictions(productId, componentId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/components/${componentId}/restrictions`);
    },

    /**
     * Retrieves the restrictions from a Product's material.
     * @param {Number} productId - Product's identifier.
     * @param {*} materialId - Material's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the material's restrictions.
     */
    getProductMaterialRestrictions(productId, materialId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/materials/${materialId}/restrictions`);
    },

    /**
     * Adds a new Product.
     * @param {*} product - Product being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added Product.
     */
    postProduct(product) {
        return Axios.post(`${PRODUCTS_URL}`, product);
    },

    /**
     * Adds a new dimension to a Product.
     * @param {Number} productId - Product's identifier.
     * @param {*} dimension - Dimension being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added dimension.
     */
    postProductDimension(productId, dimension) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/dimensions`, dimension);
    },

    /**
     * Adds a new component to a Product.
     * @param {Number} productId - Product's identifier.
     * @param {*} component - Component being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added component.
     */
    postProductComponent(productId, component) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/components`, component);
    },


    /**
     * Adds a new material to a Product.
     * @param {Number} productId - Product's identifier.
     * @param {*} material - Material being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added material.
     */
    postProductMaterial(productId, material) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/materials`, material);
    },


    /**
     * Adds a new restriction to a Product's dimension.
     * @param {Number} productId - Product's identifier.
     * @param {Number} dimensionId - Dimension's identifier.
     * @param {*} restriction - Restriction being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added restriction.
     */
    postProductDimensionRestriction(productId, dimensionId, restriction) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}/restrictions`, restriction);
    },

    /**
     * Adds a new restriction to a Product's component.
     * @param {Number} productId - Product's identifier.
     * @param {Number} componentId - Component's identifier.
     * @param {*} restriction - Restriction being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added restriction.
     */
    postProductComponentRestriction(productId, componentId, restriction) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/components/${componentId}/restrictions`, restriction);
    },

    /**
     * Adds a new restriction to a Product's material.
     * @param {Number} productId - Product's identifier.
     * @param {Number} materialId - Material's identifier.
     * @param {*} restriction - Restriction being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added restriction.
     */
    postProductMaterialRestriction(productId, materialId, restriction) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/materials/${materialId}/restrictions`, restriction);
    },

    /**
     * Updates a Product.
     * @param {Number} productId - Product's identifier. 
     * @param {*} product - Product's updated data.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated Product.
     */
    putProduct(productId, product) {
        return Axios.put(`${PRODUCTS_URL}/${productId}`, product);
    },

    /**
     * Deletes a Product.
     * @param {Number} productId - Product's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Product.
     */
    deleteProduct(productId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}`);
    },

    /**
     * Deletes a Product's dimension.
     * @param {Number} productId - Product's identifier.
     * @param {Number} dimensionId - Dimensions' identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted dimension.
     */
    deleteProductDimension(productId, dimensionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}`);
    },

    /**
     * Deletes a Product's component.
     * @param {Number} productId - Product's identifier.
     * @param {Number} componentId - Component's identifier.
     * @returns {AxiosPromise<any>}  Axios Promise representing the deleted component.
     */
    deleteProductComponent(productId, componentId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/components/${componentId}`);
    },

    /**
     * Deletes a Product's material.
     * @param {Number} productId - Product's identifier.
     * @param {Number} materialId - Material's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted material.
     */
    deleteProductMaterial(productId, materialId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/materials/${materialId}`);
    },


    /**
     * Deletes a restriction from a Product's dimension.
     * @param {Number} productId - Product's identifier.
     * @param {Number} dimensionId - Dimension's identifier.
     * @param {Number} restrictionId - Restriction's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted restriction.
     */
    deleteProductDimensionRestriction(productId, dimensionId, restrictionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}/restrictions/${restrictionId}`);
    },


    /**
     * Deletes a restriction from a Product's component.
     * @param {Number} productId - Product's identifier.
     * @param {Number} componentId - Component's identifier.
     * @param {Number} restrictionId - Restriction's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted restriction.
     */
    deleteProductComponentRestriction(productId, componentId, restrictionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/components/${componentId}/restrictions/${restrictionId}`);
    },

    /**
     * Deletes a restriction from a Product's material.
     * @param {Number} productId - Product's identifier.
     * @param {Number} materialId - Material's identifier.
     * @param {Number} restrictionId - Restriction's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted restriction.
     */
    deleteProductMaterialRestriction(productId, materialId, restrictionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/materials/${materialId}/restrictions/${restrictionId}`);
    }
}