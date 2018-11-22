using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using core.persistence;
using System;
using Microsoft.EntityFrameworkCore;

namespace backend.persistence.ef
{
    public class EFMaterialRepository : EFBaseRepository<Material, long, string>, MaterialRepository
    {
        public EFMaterialRepository(MyCContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Returns an enumerable of materials based on their identifiers
        /// </summary>
        /// <param name="materialsDTO">IEnumerable with the materials identifiers information</param>
        /// <returns>IEnumerable with the materials based on their identifiers</returns>
        public IEnumerable<Material> getMaterialsByIDS(IEnumerable<MaterialDTO> materialsDTO)
        {
            List<long> materialsIDS = new List<long>();
            foreach (MaterialDTO materialDTO in materialsDTO) materialsIDS.Add(materialDTO.id);
            return (from material in base.dbContext.Set<Material>()
                    where materialsIDS.Contains(material.Id)
                    select material).ToList();
        }
        /// <summary>
        /// Returns the material without the finish
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="idFinish">id of the finish to be remove</param>
        /// <returns>Material without the finish</returns>
        public Material deleteFinish(long idMaterial, long idFinish)
        {

            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(idMaterial);

            List<Finish> finishes =material.Finishes;

            foreach (Finish finish in finishes.ToList())
            {
                if (finish.Id.Equals(idFinish))
                {
                    material.removeFinish(finish);
                }
            }

            return material;

        }
        /// <summary>
        /// Returns the material without the color
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="idColor">id of the color to be remove</param>
        /// <returns>Material without the color</returns>

        public Material deleteColor(long idMaterial, long idColor)
        {

            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(idMaterial);

            List<Color> colors =material.Colors;

            foreach (Color color in colors.ToList())
            {
                if (color.Id.Equals(idColor))
                {
                    material.removeColor(color);
                }
            }

            return material;

        }
    }
}