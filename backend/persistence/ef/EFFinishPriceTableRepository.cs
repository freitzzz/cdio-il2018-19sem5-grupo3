using core.domain;
using core.dto;
using core.persistence;
using System.Collections.Generic;
using System.Linq;

namespace backend.persistence.ef
{
    /// <summary>
    /// EF Repository for Finish Price Table Entry instances
    /// </summary>
    /// <typeparam name="FinishPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID of the entity</typeparam>
    /// <typeparam name="FinishPriceTableEntry">Entity identifier</typeparam>
    public class EFFinishPriceTableRepository : EFBaseRepository<FinishPriceTableEntry, long, string>, FinishPriceTableRepository
    {
        public EFFinishPriceTableRepository(MyCContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO with the information about the fetch</param>
        /// <returns>IEnumerable with the material finish price history</returns>
        public IEnumerable<FinishPriceTableEntry> fetchMaterialFinishPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO){
            return (from finishPriceTableEntry in base.dbContext.FinishPriceTable
                    from materialPriceTableEntry in dbContext.MaterialPriceTable
                    where materialPriceTableEntry.Id==fetchMaterialFinishPriceHistoryDTO.materialID
                    where finishPriceTableEntry.Id==fetchMaterialFinishPriceHistoryDTO.finishID
                    select finishPriceTableEntry);
        }
    }
}