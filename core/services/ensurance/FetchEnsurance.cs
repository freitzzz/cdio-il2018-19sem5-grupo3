using core.domain;
using core.dto;
using core.persistence;
using support.utils;
using System;
using System.Collections.Generic;

namespace core.services.ensurance{
    /// <summary>
    /// Service class for ensuring that a variety of fetches are sucessful by their goals
    /// </summary>
    public sealed class FetchEnsurance{
        
        /// <summary>
        /// Constant that represents the message that occurs if the product being fetched doesn't exist
        /// </summary>
        public const string INVALID_PRODUCT_FETCH="The product being fetched doesn't exist";

        /// <summary>
        /// Constant that represents the message that occurs if the product category being fetched doesn't exist
        /// </summary>
        public const string INVALID_PRODUCT_CATEGORY_FETCH="The product being fetched doesn't exist";

        /// <summary>
        /// Constant that represents the message that occurs if the materials being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_MATERIALS_FETCH = "The materials being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the components being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_COMPONENTS_FETCH = "The components being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the products being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_PRODUCTS_FETCH = "The products being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the dimensions being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_DIMENSIONS_FETCH = "The dimensions being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occurs if the slots customized being fetched
        /// are invalid
        /// </summary>
        private const string INVALID_SLOT_CUSTOMIZED_PRODUCTS_FETCH = "The slots customized products being fetched are invalid";

        /// <summary>
        /// Ensures that a product fetch was successful
        /// </summary>
        /// <param name="product">Product with the fetched product</param>
        public static void ensureProductFetchWasSuccessful(Product product){
            if(product==null)
                throw new InvalidOperationException(INVALID_PRODUCT_FETCH);
        }

        /// <summary>
        /// Ensures that a product category fetch was successful
        /// </summary>
        /// <param name="productCategory">ProductCategory with the fetched product category</param>
        public static void ensureProductCategoryFetchWasSuccessful(ProductCategory productCategory){
            if(productCategory==null)
                throw new InvalidOperationException(INVALID_PRODUCT_CATEGORY_FETCH);
        }

        /// <summary>
        /// Ensures that the materials fetch was successful
        /// </summary>
        /// <param name="materialsToFetch">IEnumerable with the materials dtos to fetch</param>
        /// <param name="fetchedMaterials">IEnumerable with the fetched materials</param>
        public static void ensureMaterialsFetchWasSuccessful(IEnumerable<MaterialDTO> materialsToFetch, IEnumerable<Material> fetchedMaterials) {
            if (Collections.isEnumerableNullOrEmpty(materialsToFetch) || Collections.getEnumerableSize(materialsToFetch) != Collections.getEnumerableSize(fetchedMaterials))
                throw new InvalidOperationException(INVALID_MATERIALS_FETCH);
        }

        /// <summary>
        /// Ensures that the produts components fetch was successful
        /// </summary>
        /// <param name="componentsToFetch">IEnumerable with the components dtos to fetch</param>
        /// <param name="fetchedComponents">IEnumerable with the fetched components</param>
        public static void ensureProductsComponentsFetchWasSuccesful(IEnumerable<ComponentDTO> componentsToFetch, IEnumerable<Product> fetchedComponents) {
            if (Collections.isEnumerableNullOrEmpty(componentsToFetch) || Collections.getEnumerableSize(componentsToFetch) != Collections.getEnumerableSize(fetchedComponents))
                throw new InvalidOperationException(INVALID_COMPONENTS_FETCH);
        }

        /// <summary>
        /// Ensures that the products fetch was successful
        /// </summary>
        /// <param name="productsToFetch">IEnumerable with the products dtos to fetch</param>
        /// <param name="fetchedProducts">IEnumerable with the fetched products</param>
        public static void ensureProductsFetchWasSuccesful(IEnumerable<ProductDTO> productsToFetch, IEnumerable<Product> fetchedProducts) {
            if (Collections.isEnumerableNullOrEmpty(productsToFetch) || Collections.getEnumerableSize(productsToFetch) != Collections.getEnumerableSize(fetchedProducts))
                throw new InvalidOperationException(INVALID_PRODUCTS_FETCH);
        }

        /// <summary>
        /// Ensures that the products dimensions fetch was successful
        /// </summary>
        /// <param name="dimensionsToFetch">IEnumerable with the dimensions dtos to fetch</param>
        /// <param name="fetchedDimensions">IEnumerable with the fetched dimensions</param>
        public static void ensureProductsDimensionsFetchWasSuccesful(IEnumerable<DimensionDTO> dimensionsToFetch, IEnumerable<Dimension> fetchedDimensions) {
            if (Collections.isEnumerableNullOrEmpty(dimensionsToFetch) || Collections.getEnumerableSize(dimensionsToFetch) != Collections.getEnumerableSize(fetchedDimensions))
                throw new InvalidOperationException(INVALID_DIMENSIONS_FETCH);
        }

        /// <summary>
        /// Ensures that the slots customized products fetch was successful
        /// </summary>
        /// <param name="customizedProductsToFetch">IEnumerable with the customized products dtos to fetch</param>
        /// <param name="fetchedCustomizedProducts">IEnumerable with the fetched customized products</param>
        public static void ensureSlotsCustomizedProductsFetchWasSuccessful(IEnumerable<CustomizedProductDTO> customizedProductsToFetch, IEnumerable<CustomizedProduct> fetchedCustomizedProducts){
            if(Collections.isEnumerableNullOrEmpty(customizedProductsToFetch) || Collections.getEnumerableSize(customizedProductsToFetch) != Collections.getEnumerableSize(fetchedCustomizedProducts))
                throw new InvalidOperationException(INVALID_SLOT_CUSTOMIZED_PRODUCTS_FETCH);
        }
    }
}