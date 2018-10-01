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

    }
}