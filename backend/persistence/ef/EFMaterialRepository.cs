using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using core.persistence;
using System;
using Microsoft.EntityFrameworkCore;

namespace backend.persistence.ef {
    public class EFMaterialRepository : EFBaseRepository<Material, long, string>, MaterialRepository {
        public EFMaterialRepository(MyCContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Returns an enumerable of materials based on their identifiers
        /// </summary>
        /// <param name="materialsDTO">IEnumerable with the materials identifiers information</param>
        /// <returns>IEnumerable with the materials based on their identifiers</returns>
        public IEnumerable<Material> getMaterialsByIDS(IEnumerable<MaterialDTO> materialsDTO) {
            List<long> materialsIDS = new List<long>();
            foreach (MaterialDTO materialDTO in materialsDTO) materialsIDS.Add(materialDTO.id);
            return (from material in base.dbContext.Set<Material>()
                    where materialsIDS.Contains(material.Id)
                    select material).ToList();
        }

        public Finish findFinish(Material mat, long id) {
            DbSet<Material> materials = dbContext.Material;
            foreach (Material material in materials) {
                foreach (Finish fin in material.Finishes) {
                    if (fin.Id == id) {
                        return fin;
                    }
                }
            }
            return null;
        }
    }
}