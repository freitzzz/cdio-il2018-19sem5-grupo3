using core.domain;
using core.dto;
using core.persistence;
using support.persistence.repositories;
using System.Collections.Generic;
using System.Linq;

namespace backend.persistence.ef
{
    public class EFCustomizedProductCollectionRepository : EFBaseRepository<CustomizedProductCollection, long, string>, CustomizedProductCollectionRepository
    {
        public EFCustomizedProductCollectionRepository(MyCContext dbContext) : base(dbContext){}

        /// <summary>
        /// Fetches all available collections of customized products
        /// </summary>
        /// <returns>IEnumerable with all collections of customized products</returns>
        public IEnumerable<CustomizedProductCollection> findAllCollections()
        {
            return (from product in base.dbContext.Set<CustomizedProductCollection>()
                    where product.available==true
                    select product);
        }
    }
}