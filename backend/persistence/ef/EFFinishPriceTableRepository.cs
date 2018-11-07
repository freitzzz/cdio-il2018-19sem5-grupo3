using core.domain;
using core.persistence;

namespace backend.persistence.ef
{
    /// <summary>
    /// EF Repository for Finish Price Table Entry instances
    /// </summary>
    /// <typeparam name="FinishPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID of the entity</typeparam>
    /// <typeparam name="FinishPriceTableEntry">Entity identifier</typeparam>
    public class EFFinishPriceTableRepository : EFBaseRepository<FinishPriceTableEntry, long, FinishPriceTableEntry>, FinishPriceTableRepository
    {
        public EFFinishPriceTableRepository(MyCContext dbContext) : base(dbContext) { }
    }
}