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

        public IEnumerable<ProductCategory> findSubCategories(ProductCategory category)
        {
            return dbContext.ProductCategory.Where(c => c.parentId == category.Id).Where(c => c.activated).ToList();
        }

        public IEnumerable<ProductCategory> findLeaves()
        {
            List<long> allIds = dbContext.ProductCategory.Select(c => c.Id).ToList();
            List<long> leafIds = new List<long>();

            foreach(long categoryId in allIds){
               List<long> childrenIds =  dbContext.ProductCategory.Where(c => c.parentId == categoryId).Where(c => c.activated).Select(c => c.Id).ToList();
                if(!childrenIds.Any()){
                    leafIds.Add(categoryId);
                }
            }

            return dbContext.ProductCategory.Where(c => leafIds.Contains(c.Id)).ToList();

            /* //fetch all active parent categories' ids
            var ids = dbContext.ProductCategory.Where(c => c.parentId != null).Select(c => c.parentId).Distinct().ToList();
            
            return dbContext.ProductCategory.Where(c => !ids.Contains(c.Id)).Where(c => c.parentId != null).ToList(); */
        }

        public bool isLeaf(ProductCategory category)
        {
           return !findSubCategories(category).Any();
        }
    }
}