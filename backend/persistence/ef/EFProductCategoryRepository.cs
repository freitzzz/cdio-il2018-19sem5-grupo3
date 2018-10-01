using System.Collections.Generic;
using core.domain;
using core.persistence;
using support.persistence.repositories;

namespace backend.persistence.ef
{
    public class EFProductCategoryRepository : EFBaseRepository<ProductCategory, long, string>, ProductCategoryRepository
    {
        public EFProductCategoryRepository(MyCContext dbContext) : base(dbContext)
        {
        }
    }
}