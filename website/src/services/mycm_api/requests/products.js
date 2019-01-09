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
     * @param {number} productId - Product's identifier.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product. 
     */
    getProductById(productId, unit) {

        var requestUrl = `${PRODUCTS_URL}/${productId}`;

        if (unit !== undefined) {
          requestUrl = requestUrl.concat(`/?unit=${unit}`);
        }

        return Axios.get(requestUrl);
    },

    /**
     * Retrieves the Product with a matching reference.
     * @param {string} productReference - Product's reference.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product.
     */
    getProductByReference(productReference, unit) {

        var requestUrl = `${PRODUCTS_URL}/?reference=${productReference}`;

        if (unit !== undefined) {
           requestUrl = requestUrl.concat(`&unit=${unit}`);
        }

        return Axios.get(requestUrl);
    },

    /**
     * Retrieves the Product's dimensions.
     * @param {number} productId - Product's identifier.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product's dimensions.
     */
    getProductDimensions(productId, unit) {

        var requestUrl = `${PRODUCTS_URL}/${productId}/dimensions`;

        if (unit !== undefined) {
            requestUrl = requestUrl.concat(`/?unit=${unit}`);
        }

        return Axios.get(requestUrl);
    },

    /**
     * Retrieves the Product's components.
     * @param {number} productId - Product's identifier.
     * @param {"default" | "category"=} groupOption - Option defining how the components will be grouped together.
     * @returns {AxiosPromise<any>} Axios Promise representing the Product's components.
     */
    getProductComponents(productId, groupOption) {

        var requestUrl = `${PRODUCTS_URL}/${productId}/components`;

        if(groupOption !== undefined){
            requestUrl = requestUrl.concat(`/?groupby=${groupOption}`);
        }

        return Axios.get(requestUrl);
    },

    /**
     * Retrieves the Product's materials.
     * @param {number} productId - Product's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product's materials.
     */
    getProductMaterials(productId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/materials`)
    },

    /**
     * Retrieves the Product's slot widths.
     * @param {number} productId - Product's identifier.
     * @param {"mm" | "cm" | "dm" | "m" | "in"=} unit - Dimension unit.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved Product's slot widths.
     */
    getProductSlotWidths(productId, unit) {

        var requestUrl = `{PRODUCTS_URL}/${productId}/slotwidths`;

        if(unit !== undefined){
            requestUrl = requestUrl.concat(`?unit=${unit}`);
        }

        return Axios.get(requestUrl);
    },


    /**
     * Retrieves a Product's component.
     * @param {number} productId - Product's identifier.
     * @param {number} componentId - Component's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved component.
     */
    getProductComponent(productId, componentId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/components/${componentId}`);
    },

    /**
     * Retrieves the restrictions from a Product's dimension.
     * @param {number} productId - Product identifier.
     * @param {number} dimensionId - Dimension identifier. 
     * @returns {AxiosPromise<any>} Axios Promise representing the retrieved dimension's restrictions.
     */
    getProductDimensionRestrictions(productId, dimensionId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}/restrictions`);
    },

    /**
     * Retrieves the restrictions from a Product's component.
     * @param {number} productId - Product's identifier.
     * @param {number} componentId - Component's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the component's restrictions.
     */
    getProductComponentRestrictions(productId, componentId) {
        return Axios.get(`${PRODUCTS_URL}/${productId}/components/${componentId}/restrictions`);
    },

    /**
     * Retrieves the restrictions from a Product's material.
     * @param {number} productId - Product's identifier.
     * @param {number} materialId - Material's identifier.
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
     * @param {number} productId - Product's identifier.
     * @param {*} dimension - Dimension being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added dimension.
     */
    postProductDimension(productId, dimension) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/dimensions`, dimension);
    },

    /**
     * Adds a new component to a Product.
     * @param {number} productId - Product's identifier.
     * @param {*} component - Component being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added component.
     */
    postProductComponent(productId, component) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/components`, component);
    },


    /**
     * Adds a new material to a Product.
     * @param {number} productId - Product's identifier.
     * @param {*} material - Material being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added material.
     */
    postProductMaterial(productId, material) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/materials`, material);
    },


    /**
     * Adds a new restriction to a Product's dimension.
     * @param {number} productId - Product's identifier.
     * @param {number} dimensionId - Dimension's identifier.
     * @param {*} restriction - Restriction being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added restriction.
     */
    postProductDimensionRestriction(productId, dimensionId, restriction) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}/restrictions`, restriction);
    },

    /**
     * Adds a new restriction to a Product's component.
     * @param {number} productId - Product's identifier.
     * @param {number} componentId - Component's identifier.
     * @param {*} restriction - Restriction being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added restriction.
     */
    postProductComponentRestriction(productId, componentId, restriction) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/components/${componentId}/restrictions`, restriction);
    },

    /**
     * Adds a new restriction to a Product's material.
     * @param {number} productId - Product's identifier.
     * @param {number} materialId - Material's identifier.
     * @param {*} restriction - Restriction being added.
     * @returns {AxiosPromise<any>} Axios Promise representing the added restriction.
     */
    postProductMaterialRestriction(productId, materialId, restriction) {
        return Axios.post(`${PRODUCTS_URL}/${productId}/materials/${materialId}/restrictions`, restriction);
    },

    /**
     * Updates a Product.
     * @param {number} productId - Product's identifier. 
     * @param {*} product - Product's updated data.
     * @returns {AxiosPromise<any>} Axios Promise representing the updated Product.
     */
    putProduct(productId, product) {
        return Axios.put(`${PRODUCTS_URL}/${productId}`, product);
    },

    /**
     * Deletes a Product.
     * @param {number} productId - Product's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted Product.
     */
    deleteProduct(productId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}`);
    },

    /**
     * Deletes a Product's dimension.
     * @param {number} productId - Product's identifier.
     * @param {number} dimensionId - Dimensions' identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted dimension.
     */
    deleteProductDimension(productId, dimensionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}`);
    },

    /**
     * Deletes a Product's component.
     * @param {number} productId - Product's identifier.
     * @param {number} componentId - Component's identifier.
     * @returns {AxiosPromise<any>}  Axios Promise representing the deleted component.
     */
    deleteProductComponent(productId, componentId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/components/${componentId}`);
    },

    /**
     * Deletes a Product's material.
     * @param {number} productId - Product's identifier.
     * @param {number} materialId - Material's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted material.
     */
    deleteProductMaterial(productId, materialId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/materials/${materialId}`);
    },


    /**
     * Deletes a restriction from a Product's dimension.
     * @param {number} productId - Product's identifier.
     * @param {number} dimensionId - Dimension's identifier.
     * @param {number} restrictionId - Restriction's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted restriction.
     */
    deleteProductDimensionRestriction(productId, dimensionId, restrictionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/dimensions/${dimensionId}/restrictions/${restrictionId}`);
    },


    /**
     * Deletes a restriction from a Product's component.
     * @param {number} productId - Product's identifier.
     * @param {number} componentId - Component's identifier.
     * @param {number} restrictionId - Restriction's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted restriction.
     */
    deleteProductComponentRestriction(productId, componentId, restrictionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/components/${componentId}/restrictions/${restrictionId}`);
    },

    /**
     * Deletes a restriction from a Product's material.
     * @param {number} productId - Product's identifier.
     * @param {number} materialId - Material's identifier.
     * @param {number} restrictionId - Restriction's identifier.
     * @returns {AxiosPromise<any>} Axios Promise representing the deleted restriction.
     */
    deleteProductMaterialRestriction(productId, materialId, restrictionId) {
        return Axios.delete(`${PRODUCTS_URL}/${productId}/materials/${materialId}/restrictions/${restrictionId}`);
    }
}