using core.domain;
using core.dto;
using core.persistence;
using NodaTime;
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
        public IEnumerable<FinishPriceTableEntry> fetchMaterialFinishPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO)
        {
            return (from finishPriceTableEntry in base.dbContext.FinishPriceTable
                    where finishPriceTableEntry.entity.Id == fetchMaterialFinishPriceHistoryDTO.finishID
                    select finishPriceTableEntry);
        }

        /// <summary>
        /// Fetches the price history of all finishes of a material
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO with the information about the fetch</param>
        /// <returns>IEnumerable with the price history of all finishes of a material</returns>
        public IEnumerable<FinishPriceTableEntry> fetchAllMaterialFinishesPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO)
        {
            Material material = dbContext.Material.Where(m => m.Id == fetchMaterialFinishPriceHistoryDTO.materialID).SingleOrDefault();
            return dbContext.FinishPriceTable.Where(fpte => fpte.materialEID == material.id());
        }

        /// <summary>
        /// Fetches the current price of a material finish
        /// </summary>
        /// <param name="materialId">Material's PID</param>
        /// <param name="finishId">Finish's PID</param>
        /// <returns>FinishPriceTableEntry with the current price of a material finish</returns>
        public FinishPriceTableEntry fetchCurrentMaterialFinishPrice(long finishId)
        {
            LocalDateTime currentTime = NodaTime.LocalDateTime.FromDateTime(SystemClock.Instance.GetCurrentInstant().ToDateTimeUtc());
            List<FinishPriceTableEntry> list = dbContext.FinishPriceTable.Where(mpte => mpte.entity.Id == finishId)
                    .Where(
                            mpte => mpte.timePeriod.startingDate.CompareTo(currentTime) <= 0 &&
                            mpte.timePeriod.endingDate.CompareTo(currentTime) >= 0
                        ).ToList();
            return list.Count > 0 ? list.Last() : null;
        }
    }
}