using core.persistence;
using core.domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using support.dto;

namespace core.application
{

    /// <summary>
    /// Core MaterialsController class.
    /// </summary>
    public class MaterialsController : Controller
    {

        /// <summary>
        /// Fetches a List with all Materials present in the MaterialRepository.
        /// </summary>
        /// <returns>a List with all of the Material's DTOs</returns>
        public List<DTO> findAllMaterials()
        {
            List<DTO> dtoMaterials = new List<DTO>();
            IEnumerable<Material> materials = PersistenceContext.repositories().createMaterialRepository().findAll();

            foreach (Material material in materials)
            {
                dtoMaterials.Add(material.toDTO());
            }

            return dtoMaterials;
        }

        /// <summary>
        /// Fetches a Material from the MaterialRepository given its ID.
        /// </summary>
        /// <param name = "materialID">the Material's ID</param>
        /// <returns>DTO that represents the Material</returns>
        public DTO findMaterialByID(string materialID)
        {
            return PersistenceContext.repositories().createMaterialRepository().find(materialID).toDTO();
        }
    }
}