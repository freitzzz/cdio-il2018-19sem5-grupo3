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
        /// Finds a customized product collection by its persistence id
        /// </summary>
        /// <param name="entityPersistenceID">long with the collection of customized products persistence id</param>
        /// <returns>CustomizedProductCollection with the collection of customized products which has a certain persistence id</returns>
        public override CustomizedProductCollection find(long entityPersistenceID){
            CustomizedProductCollection customizedProductCollection=base.find(entityPersistenceID);
            return customizedProductCollection.activated ? customizedProductCollection : null;
        }

        /// <summary>
        /// Fetches all available collections of customized products
        /// </summary>
        /// <returns>IEnumerable with all collections of customized products</returns>
        public IEnumerable<CustomizedProductCollection> findAllCollections()
        {
            return (from product in base.dbContext.Set<CustomizedProductCollection>()
                    where product.activated==true
                    select product);
        }
    }
}