using core.domain;
using core.dto;
using core.persistence;
using support.utils;
using System;
using System.Collections.Generic;

namespace core.services.ensurance{
    /// <summary>
    /// Service class for ensuring a variety of operations related to product categories
    /// </summary>
    public sealed class ProductCategoryEnsurance{
        /// <summary>
        /// Constant that represents the message that occures if the product category being ensured that is a leaf 
        /// is a not leaf
        /// </summary>
        private const string INVALID_PRODUCT_CATEGORY_SUBCATEGORIES_FETCH = "The product category is not a leaf";

        /// <summary>
        /// Ensures that a product category is a leaf
        /// </summary>
        /// <param name="productCategory">ProductCategory with the product category being ensured that is leaf</param>
        public static void ensureProductCategoryIsLeaf(ProductCategory productCategory){
            IEnumerable<ProductCategory> productCategories=PersistenceContext.repositories().createProductCategoryRepository().findSubCategories(productCategory);
            if(!Collections.isEnumerableNullOrEmpty(productCategories)){
                throw new InvalidOperationException(INVALID_PRODUCT_CATEGORY_SUBCATEGORIES_FETCH);
            }
        }
    }
}