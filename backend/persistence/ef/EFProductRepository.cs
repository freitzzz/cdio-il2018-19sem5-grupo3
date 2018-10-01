using System.Collections.Generic;
using core.domain;
using core.persistence;
using support.persistence.repositories;

namespace backend.persistence.ef
{
    public class EFProductRepository : EFBaseRepository<Product, long, string>, ProductRepository
    {
        public EFProductRepository(MyCContext dbContext) : base(dbContext)
        {
        }
    }
}