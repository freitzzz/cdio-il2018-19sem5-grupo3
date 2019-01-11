using core.domain;
using core.dto;
using core.persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace backend.persistence.ef
{
    /// <summary>
    /// EF Repository for Material Price Table Entry instances
    /// </summary>
    /// <typeparam name="MaterialPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID of the entity</typeparam>
    public class EFMaterialPriceTableRepository : EFBaseRepository<MaterialPriceTableEntry, long, string>, MaterialPriceTableRepository
    {
        public EFMaterialPriceTableRepository(MyCContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Fetches the current price of a material
        /// </summary>
        /// <param name="materialId">Material's PID</param>
        /// <returns>MaterialPriceTableEntry with the material's current price</returns>
        public MaterialPriceTableEntry fetchCurrentMaterialPrice(long materialId)
        {
            LocalDateTime currentTime = NodaTime.LocalDateTime.FromDateTime(SystemClock.Instance.GetCurrentInstant().ToDateTimeUtc());
            List<MaterialPriceTableEntry> list = dbContext.MaterialPriceTable.Where(mpte => mpte.entity.Id == materialId).
                    Where(
                            mpte => mpte.timePeriod.startingDate.CompareTo(currentTime) <= 0
                            && mpte.timePeriod.endingDate.CompareTo(currentTime) >= 0
                        ).ToList();
            return list.Count > 0 ? list.Last() : null;
        }

        /// <summary>
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="fetchMaterialPriceHistoryDTO">FetchMaterialPriceHistoryDTO with the information about the fetch</param>
        /// <returns>IEnumerable with the material price history</returns>
        public IEnumerable<MaterialPriceTableEntry> fetchMaterialPriceHistory(FetchMaterialPriceHistoryDTO fetchMaterialPriceHistoryDTO)
        {
            return (from materialTableEntry in base.dbContext.MaterialPriceTable
                    where materialTableEntry.entity.Id == fetchMaterialPriceHistoryDTO.materialID
                    select materialTableEntry);
        }
    }
}