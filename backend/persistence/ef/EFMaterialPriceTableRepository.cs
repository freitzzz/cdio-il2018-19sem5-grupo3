using core.domain;
using core.persistence;

namespace backend.persistence.ef
{
    /// <summary>
    /// EF Repository for Material Price Table Entry instances
    /// </summary>
    /// <typeparam name="MaterialPriceTableEntry">Entity type</typeparam>
    /// <typeparam name="long">PID of the entity</typeparam>
    /// <typeparam name="MaterialPriceTableEntry">Entity identifier</typeparam>
    public class EFMaterialPriceTableRepository : EFBaseRepository<MaterialPriceTableEntry, long, MaterialPriceTableEntry>, MaterialPriceTableRepository
    {
        public EFMaterialPriceTableRepository(MyCContext dbContext) : base(dbContext) { }
    }
}