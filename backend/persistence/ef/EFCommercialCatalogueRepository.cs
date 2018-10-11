using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using core.persistence;
using System;
using Microsoft.EntityFrameworkCore;
namespace backend.persistence.ef
{
    public class EFCommercialCatalogueRepository : EFBaseRepository<CommercialCatalogue, long, string>, CommercialCatalogueRepository
    {
        public EFCommercialCatalogueRepository(MyCContext dbContext) : base(dbContext) { }

        //FetchCommercialCataloguesById
        /// <summary>
        /// Fetches an enumerable of Commercial Catalogues by their ids
        /// </summary>
        /// <param name="productsDTO">IEnumerable with the products information</param>
        /// <returns>IEnumerable with the fetched products</returns>
         public IEnumerable<CommercialCatalogue> fetchCommercialCataloguesById(IEnumerable<CommercialCatalogueDTO> commercialCatalogueDTO)
         {
           List<long> catalogueIDS = new List<long>();
           foreach(CommercialCatalogueDTO collectionDTO in commercialCatalogueDTO) catalogueIDS.Add(collectionDTO.id);
             return(from commercialCatalogue in base.dbContext.Set<CommercialCatalogue>()
                     where catalogueIDS.Contains(commercialCatalogue.Id)
                     select commercialCatalogue).ToList();
        }
        
    }
}