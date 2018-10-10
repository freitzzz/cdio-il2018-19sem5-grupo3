using System.Collections.Generic;
using core.domain;
using core.dto;
using support.domain.ddd;

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
        /// <param name="productsDTO">IEnumerable with the products information</param>
        /// <returns>IEnumerable with the fetched products</returns>
        IEnumerable<CommercialCatalogue> FetchCommercialCataloguesById(IEnumerable<CommercialCatalogueDTO> commercialCataloguesDTO);

    }
}
