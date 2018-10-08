using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;

namespace core.persistence{
    /// <summary>
    /// Interface that represents the repository functionalities for Product entities
    /// </summary>
    /// <typeparam name="Product">Generic-Type of the repository entity</typeparam>
    /// <typeparam name="long">Generic-Type of the repository persistence ID</typeparam>
    /// <typeparam name="string">Generic-Type of the repository entity identifier</typeparam>
    public interface ProductRepository:Repository<Product,long,string>{
        /// <summary>
        /// Fetches an enumerable of products by their ids
        /// </summary>
        /// <param name="productsDTO">IEnumerable with the products information</param>
        /// <returns>IEnumerable with the fetched products</returns>
        IEnumerable<Product> fetchProductsByID(IEnumerable<ProductDTO> productsDTO);
    }
}