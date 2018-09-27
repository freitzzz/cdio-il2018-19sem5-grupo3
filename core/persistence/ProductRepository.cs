using core.domain;
using support.domain.ddd;
using support.persistence.repositories;

namespace core.persistence{
    /// <summary>
    /// Interface that represents the repository functionalities for Product entities
    /// </summary>
    /// <typeparam name="Product">Generic-Type of the repository entity</typeparam>
    /// <typeparam name="string">Generic-Type of the repository persistence ID</typeparam>
    /// <typeparam name="string">Generic-Type of the repository entity identifier</typeparam>
    public interface ProductRepository:Repository<Product,string,string>,DataRepository<Product,string>{}
}