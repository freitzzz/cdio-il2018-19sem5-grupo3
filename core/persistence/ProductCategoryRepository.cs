using System.Collections.Generic;
using core.domain;
using support.domain.ddd;
using support.persistence.repositories;

namespace core.persistence
{
    /// <summary>
    /// Interface that represents the repository functionalities for ProductCategory entities
    /// </summary>
    /// <typeparam name="ProductCategory">Type of Entity being stored in this repository</typeparam>
    /// <typeparam name="long">Entity's database identifier</typeparam>
    /// <typeparam name="string">Entity's domain identifier</typeparam>
    public interface ProductCategoryRepository : Repository<ProductCategory, long, string>
    {
        /// <summary>
        /// Finds all ProductCategories' sub-categories (ProductCategories that have the received category as a parent).
        /// </summary>
        /// <param name="category">Category to search</param>
        /// <returns>List with all ProductCategory sub-categories</returns>
        IEnumerable<ProductCategory> findSubCategories(ProductCategory category);

        /// <summary>
        /// Finds all ProductCategories that are leaves (that aren't parent of any other ProductCategory).
        /// </summary>
        /// <returns>List with all ProductCategory leaves</returns>
        IEnumerable<ProductCategory> findLeaves();
    }
}