using System.Collections.Generic;
using core.domain;
using core.persistence;
using support.persistence.repositories;
using System.Linq;

namespace backend.persistence.ef
{
    public class EFProductCategoryRepository : EFBaseRepository<ProductCategory, long, string>, ProductCategoryRepository
    {
        public EFProductCategoryRepository(MyCContext dbContext) : base(dbContext) { }


        public override IEnumerable<ProductCategory> findAll()
        {
            return dbContext.ProductCategory.Where(c => c.active).ToList();
        }

        public override ProductCategory find(string entityID)
        {
            return dbContext.ProductCategory.Where(c => c.active).Where(c => c.sameAs(entityID)).SingleOrDefault();
        }

        public override ProductCategory find(long entityPersistenceID)
        {
            return dbContext.ProductCategory.Where(c => c.active).Where(c => c.Id == entityPersistenceID).SingleOrDefault();
        }

        public override ProductCategory remove(ProductCategory entity){

            entity.deactivate();
            dbContext.SaveChanges();

            return entity;
        }

    }
}