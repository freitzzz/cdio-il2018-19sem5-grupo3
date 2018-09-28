using core.domain;
using core.persistence;

namespace backend.persistence.ef
{
    public class EFMaterialRepository : EFBaseRepository<Material, long, string>, MaterialRepository
    {
        public EFMaterialRepository(MyCContext dbContext) : base(dbContext)
        {
        }
    }
}