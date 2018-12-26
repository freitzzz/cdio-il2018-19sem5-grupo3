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
        public EFCustomizedProductCollectionRepository(MyCContext dbContext) : base(dbContext) { }

    }
}