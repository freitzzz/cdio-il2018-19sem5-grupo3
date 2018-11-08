using core.domain;
using support.domain.ddd;
using support.persistence.repositories;

namespace core.persistence
{
    /// <summary>
    /// Interface that represents the repository functionalities for FinishPriceTableEntry entities
    /// </summary>
    /// <typeparam name="FinishPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID type</typeparam>
    /// <typeparam name="FinishPriceTableEntry">Entity's Business Identifier</typeparam>
    public interface FinishPriceTableRepository : Repository<FinishPriceTableEntry, long, string>
    {

    }
}