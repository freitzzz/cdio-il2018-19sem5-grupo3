using core.domain;
using core.persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace backend.persistence.ef
{
    /// <summary>
    /// EF Repository for Material Price Table Entry instances
    /// </summary>
    /// <typeparam name="MaterialPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID of the entity</typeparam>
    /// <typeparam name="MaterialPriceTableEntry">Entity identifier</typeparam>
    public class EFMaterialPriceTableRepository : EFBaseRepository<MaterialPriceTableEntry, long, string>, MaterialPriceTableRepository
    {
        public EFMaterialPriceTableRepository(MyCContext dbContext) : base(dbContext) { }
    }
}