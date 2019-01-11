using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;

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
        /// <summary>
        /// Fetches the price history of a material
        /// </summary>
        /// <param name="fetchMaterialPriceHistoryDTO">FetchMaterialPriceHistoryDTO with the information about the fetch</param>
        /// <returns>IEnumerable with the material price history</returns>
        IEnumerable<MaterialPriceTableEntry> fetchMaterialPriceHistory(FetchMaterialPriceHistoryDTO fetchMaterialPriceHistoryDTO);
    
        /// <summary>
        /// Fetches the current price of a material
        /// </summary>
        /// <param name="materialId">Material's PID</param>
        /// <returns>MaterialPriceTableEntry with the material's current price</returns>
        MaterialPriceTableEntry fetchCurrentMaterialPrice(long materialId);
    }
}