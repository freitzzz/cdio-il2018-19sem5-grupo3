using core.domain;
using support.persistence.repositories;

namespace core.persistence
{
    /// <summary>
    /// Interface that represents the repository functionalities for MaterialPriceTableEntry entities
    /// </summary>
    /// <typeparam name="MaterialPriceTableEntry">Entity Type</typeparam>
    /// <typeparam name="long">PID type</typeparam>
    public interface MaterialPriceTableRepository : DataRepository<MaterialPriceTableEntry,long>
    {

    }
}