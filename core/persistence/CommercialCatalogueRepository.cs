using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;

namespace core.persistence
{
    /// <summary>
    /// Interface that represents the repository functionalities for CommercialCatalogue entities
    /// </summary>
    /// <typeparam name="CommercialCatalogue">Type of Entity being stored in this repository</typeparam>
    /// <typeparam name="long">Entity's database identifier</typeparam>
    /// <typeparam name="string">Entity's domain identifier</typeparam>
    public interface CommercialCatalogueRepository : Repository<CommercialCatalogue, long, string>
    {

        //FetchCommercialCataloguesById
        /// <summary>
        /// Fetches an enumerable of Commercial Catalogues by their ids
        /// </summary>
        /// <param name="commercialCataloguesDTO">IEnumerable with the commercialCatalogues information</param>
        /// <returns>IEnumerable with the fetched commercialCatalogues</returns>
        IEnumerable<CommercialCatalogue> fetchCommercialCataloguesById(IEnumerable<CommercialCatalogueDTO> commercialCataloguesDTO);

    }
}
