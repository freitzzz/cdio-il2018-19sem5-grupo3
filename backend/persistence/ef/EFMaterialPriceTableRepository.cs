using core.domain;
using core.dto;
using core.persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace backend.persistence.ef
{
    /// <summary>
    /// EF Repository for Material Price Table Entry instances
    /// </summary>
    /// <typeparam name="MaterialPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID of the entity</typeparam>
    /// /// <typeparam name="MaterialPriceTableEntry">Entity identifier</typeparam>
    public class EFMaterialPriceTableRepository : EFBaseRepository<MaterialPriceTableEntry, long, string>, MaterialPriceTableRepository
    {
        public EFMaterialPriceTableRepository(MyCContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="fetchMaterialPriceHistoryDTO">FetchMaterialPriceHistoryDTO with the information about the fetch</param>
        /// <returns>IEnumerable with the material price history</returns>
        public IEnumerable<MaterialPriceTableEntry> fetchMaterialPriceHistory(FetchMaterialPriceHistoryDTO fetchMaterialPriceHistoryDTO){
            return (from materialTableEntry in base.dbContext.MaterialPriceTable
                    where materialTableEntry.entity.Id==fetchMaterialPriceHistoryDTO.materialID
                    select materialTableEntry);
        }
    }
}