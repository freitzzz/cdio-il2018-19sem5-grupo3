using core.domain;
using core.dto;
using core.persistence;
using System.Collections.Generic;

namespace backend.persistence.ef {
    public class EFMaterialRepository : EFBaseRepository<Material, long, string>, MaterialRepository {
        public EFMaterialRepository(MyCContext dbContext) : base(dbContext) {
        }

        public List<Finish> findFinishes(Material mat, List<FinishDTO> finishes) {
            List<Finish> fins = new List<Finish>();
            foreach (FinishDTO dto in finishes) {
                List<Finish> query = from m in Material
                                     where m.id = mat.id()
                                     select m;
            }
            return fins;
        }

        public Finish findFinish(Material mat, long id) {
            Finish query = from m in Material
                           where m.Id = mat.Id
                           select m;
            return null;
        }
    }
}