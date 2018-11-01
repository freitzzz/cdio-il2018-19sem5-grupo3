using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;

namespace core.persistence {
    /// <summary>
    /// Interface that represents the repository functionalities for Product entities
    /// </summary>
    /// <typeparam name="Product">Generic-Type of the repository entity</typeparam>
    /// <typeparam name="long">Generic-Type of the repository persistence ID</typeparam>
    /// <typeparam name="string">Generic-Type of the repository entity identifier</typeparam>
    public interface ProductRepository : Repository<Product, long, string> {
        /// <summary>
        /// Fetches an enumerable of products by their ids
        /// </summary>
        /// <param name="productsDTO">IEnumerable with the products information</param>
        /// <returns>IEnumerable with the fetched products</returns>
        IEnumerable<Product> fetchProductsByID(IEnumerable<ProductDTO> productsDTO);
        /// <summary>
        /// Fetches a width dimension of a product
        /// </summary>
        /// <param name="fetchProductDimensionDTO">FetchProductDimensionDTO with the product width dimension information</param>
        /// <returns>Dimension with the fetched product width dimension</returns>
        Dimension fetchProductWidthDimension(FetchProductDimensionDTO fetchProductDimensionDTO);
        
        /// <summary>
        /// Fetches a height dimension of a product
        /// </summary>
        /// <param name="fetchProductDimensionDTO">FetchProductDimensionDTO with the product height dimension information</param>
        /// <returns>Dimension with the fetched product height dimension</returns>
        Dimension fetchProductHeightDimension(FetchProductDimensionDTO fetchProductDimensionDTO);

        /// <summary>
        /// Fetches a depth dimension of a product
        /// </summary>
        /// <param name="fetchProductDimensionDTO">FetchProductDimensionDTO with the product depth dimension information</param>
        /// <returns>Dimension with the fetched product depth dimension</returns>
        Dimension fetchProductDepthDimension(FetchProductDimensionDTO fetchProductDimensionDTO);
        /// <summary>
        /// Fetches product component by their ids
        /// </summary>
        /// <param name="productID">product id</param>
        /// <param name="componentID">component id</param>
        /// <returns>product component with respective id</returns>
        Component fetchProductComponent(long productID, long componentID);
    }
}