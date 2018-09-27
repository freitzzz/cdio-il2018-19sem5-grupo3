using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;

namespace core.application
{

    /// <summary>
    /// Core MaterialsController class.
    /// </summary>
    public class MaterialsController
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

        /// <summary>
        /// Removes a Material from the MaterialRepository given its ID.
        /// </summary>
        /// <param name="materialID">the Material's ID</param>
        /// <returns>DTO that represents the Material</returns>
        public DTO removeMaterial(string materialID)
        {
            Material material = PersistenceContext.repositories().createMaterialRepository().find(materialID);
            return PersistenceContext.repositories().createMaterialRepository().remove(material).toDTO();
        }

        /// <summary>
        /// Adds a Material to the MaterialRepository given its ID.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>DTO that represents the Material</returns>
        //public DTO addMaterial(DTO materialDTO)
        //{
            //TODO Extract data from DTO and add new Material to Database
            //return PersistenceContext.repositories().createMaterialRepository().save(material).toDTO();
        //}

        /// <summary>
        /// Updates the Material on the MaterialRepository given its data.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>DTO that represents the updated Material</returns>
        //public DTO updateMaterial(DTO materialDTO){
            //TODO Extract Database ID and new data from DTO
            //return PersistenceContext.repositories().createMaterialRepository().update(material).toDTO();
        //}
        /// Parses an enumerable of materials persistence identifiers as an enumerable of entities
        /// </summary>
        /// <param name="materialsIDS">Enumerable with the materials persistence identifiers</param>
        /// <returns>IEnumerable with the materials ids as entities</returns>
        internal IEnumerable<Material> enumerableOfMaterialsIDSAsEntities(IEnumerable<long> materialsIDS){
            if(materialsIDS==null)return null;
            List<Material> materials=new List<Material>();
            MaterialRepository materialRepository=PersistenceContext.repositories().createMaterialRepository();
            IEnumerator<long> materialsIDSEnumerator=materialsIDS.GetEnumerator();
            long nextMaterialID=materialsIDSEnumerator.Current;
            while(materialsIDSEnumerator.MoveNext()){
                nextMaterialID=materialsIDSEnumerator.Current;
                //Uncomment when Material Persistence ID is changed to long
                //materials.Add(materialRepository.find(nextMaterialID));
            }
            return materials;
        }
    }
}