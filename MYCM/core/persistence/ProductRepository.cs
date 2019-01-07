using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// Finds all instances of Product that are not complementary to any other instances of Product.
        /// </summary>
        /// <returns>An IEnumerable of Product containing all base products.</returns>
        IEnumerable<Product> findBaseProducts();

        /// <summary>
        /// Checks if a Product with the given identifier is a base.
        /// </summary>
        /// <param name="productId">Product's identifier persistence identifier.</param>
        /// <returns>true, if the Product with the given identifier is a base; false, otherwise.</returns>
        bool isBaseProduct(long productId);
    }
}