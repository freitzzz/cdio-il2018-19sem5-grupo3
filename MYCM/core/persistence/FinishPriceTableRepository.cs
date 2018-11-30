using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;

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
        /// <summary>
        /// Fetches the price history of a material finish
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO with the information about the fetch</param>
        /// <returns>IEnumerable with the material finish price history</returns>
        IEnumerable<FinishPriceTableEntry> fetchMaterialFinishPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO);
    
        /// <summary>
        /// Fetches the price history of all finishes of a material
        /// </summary>
        /// <param name="fetchMaterialFinishPriceHistoryDTO">FetchMaterialFinishPriceHistoryDTO with the material's PID</param>
        /// <returns>IEnumerable with the price history of all finishes of a material</returns>
        IEnumerable<FinishPriceTableEntry> fetchAllMaterialFinishesPriceHistory(FetchMaterialFinishPriceHistoryDTO fetchMaterialFinishPriceHistoryDTO);
    }
}