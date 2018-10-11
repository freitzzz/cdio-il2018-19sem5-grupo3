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

        public override ProductCategory remove(ProductCategory entity)
        {

            entity.deactivate();
            dbContext.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Finds all ProductCategories' sub-categories (ProductCategories that have the received category as a parent).
        /// </summary>
        /// <param name="category">Category to search</param>
        /// <returns>List with all ProductCategory sub-categories</returns>
        public IEnumerable<ProductCategory> findSubCategories(ProductCategory category)
        {
            return dbContext.ProductCategory.Where(c => c.parentId == category.Id).Where(c => c.active).ToList();
        }

        /// <summary>
        /// Finds all ProductCategories that are leaves (that aren't parent of any other ProductCategory).
        /// </summary>
        /// <returns>List with all ProductCategory leaves</returns>
        public IEnumerable<ProductCategory> findLeaves()
        {
            //fetch all active parent categories' ids
            var ids = dbContext.ProductCategory.Where(c => c.parentId != null).Select(c => c.parentId).Distinct().ToList();
            
            return dbContext.ProductCategory.Where(c => !ids.Contains(c.Id)).Where(c => c.parentId != null).ToList();
        }
    }
}