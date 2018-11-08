using core.domain;
using support.domain.ddd;
using support.persistence.repositories;

namespace core.persistence
{
    /// <summary>
    /// Interface that represents the repository functionalities for MaterialPriceTableEntry entities
    /// </summary>
    /// <typeparam name="MaterialPriceTableEntry">Entity Type</typeparam>
    /// <typeparam name="long">PID type</typeparam>
    /// <typeparam name="MaterialPriceTableEntry">Entity's Business Identifier</typeparam>
    public interface MaterialPriceTableRepository : Repository<MaterialPriceTableEntry, long, string>
    {

    }
}