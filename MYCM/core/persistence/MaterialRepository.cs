using core.domain;
using core.dto;
using support.domain.ddd;
using support.persistence.repositories;
using System.Collections.Generic;

namespace core.persistence {
    ///<summary>Interface that represents the repository functionalities for Material entities.</summary>
    ///<typeparam name = "Material">Generic-Type of the repository entity</typeparam>
    ///<typeparam name = "string">Generic-Type of the repository persistence ID</typeparam>
    public interface MaterialRepository : Repository<Material, long, string> {
        /// <summary>
        /// Returns an enumerable of materials based on their identifiers
        /// </summary>
        /// <param name="materialsDTO">IEnumerable with the materials identifiers information</param>
        /// <returns>IEnumerable with the materials based on their identifiers</returns>
        IEnumerable<Material> getMaterialsByIDS(IEnumerable<MaterialDTO> materialsDTO);

        Material deleteColor(long idMaterial, long idColor);

        Material deleteFinish(long idMaterial, long idFinish);

    }
}