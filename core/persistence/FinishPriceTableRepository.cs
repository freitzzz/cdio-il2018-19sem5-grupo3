using core.domain;
using support.persistence.repositories;

namespace core.persistence
{
    /// <summary>
    /// Interface that represents the repository functionalities for FinishPriceTableEntry entities
    /// </summary>
    /// <typeparam name="FinishPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID type</typeparam>
    public interface FinishPriceTableRepository : DataRepository<FinishPriceTableEntry, long>
    {

    }
}