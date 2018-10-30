using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;
using core.dto;
using core.services;

namespace core.application
{

    /// <summary>
    /// Core MaterialsController class.
    /// </summary>
    public class MaterialsController
    {

        /// <summary>
        /// Builds a new MaterialsController
        /// </summary>
        public MaterialsController() { }

        /// <summary>
        /// Adds a new material
        /// </summary>
        /// <param name="materialAsDTO">DTO with the material information</param>
        /// <returns>DTO with the created material DTO, null if the material was not created</returns>
        public MaterialDTO addMaterial(MaterialDTO materialAsDTO)
        {
            Material newMaterial = new MaterialDTOService().transform(materialAsDTO);
            Material createdMaterial = PersistenceContext.repositories().createMaterialRepository().save(newMaterial);
            if (createdMaterial == null) return null;
            return createdMaterial.toDTO();
        }

        /// <summary>
        /// Updates basic information of a material
        /// </summary>
        /// <param name="updateMaterialDTO">UpdateMaterialDTO with the data regarding the material update</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateMaterialBasicInformation(UpdateMaterialDTO updateMaterialDTO)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material materialBeingUpdated = materialRepository.find(updateMaterialDTO.id);
            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;
            if (updateMaterialDTO.reference != null)
            {
                updatedWithSuccess &= materialBeingUpdated.changeReference(updateMaterialDTO.reference);
                perfomedAtLeastOneUpdate = true;
            }
            if (updateMaterialDTO.designation != null)
            {
                updatedWithSuccess &= materialBeingUpdated.changeDesignation(updateMaterialDTO.designation);
                perfomedAtLeastOneUpdate = true;
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;
            updatedWithSuccess &= materialRepository.update(materialBeingUpdated) != null;
            return updatedWithSuccess;
        }
        /// <summary>
        /// Disables a material
        /// </summary>
        /// <param name="materialDTO">MaterialDTO with the material data being disabled</param>
        /// <returns>boolean true if the material was disabled with success, false if not</returns>
        public bool disableMaterial(MaterialDTO materialDTO)
        {
            Material materialBeingDisabled = PersistenceContext.repositories().createMaterialRepository().find(materialDTO.id);
            return materialBeingDisabled != null && materialBeingDisabled.deactivate();
        }

        /// <summary>
        /// Removes (Disables) a material
        /// </summary>
        /// <param name="materialDTO">DTO with the material information</param>
        /// <returns>boolean true if the material was removed (disabled) with success</returns>
        public bool removeMaterial(MaterialDTO materialDTO)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material materialBeingRemoved = materialRepository.find(materialDTO.id);
            return materialBeingRemoved != null && materialBeingRemoved.deactivate() && materialRepository.update(materialBeingRemoved) != null;
        }

        /// <summary>
        /// Fetches a List with all Materials present in the MaterialRepository.
        /// </summary>
        /// <returns>a List with all of the Material's DTOs</returns>
        public List<MaterialDTO> findAllMaterials()
        {
            List<MaterialDTO> dtoMaterials = new List<MaterialDTO>();
            IEnumerable<Material> materials = PersistenceContext.repositories().createMaterialRepository().findAll();

            if (materials == null || !materials.GetEnumerator().MoveNext())
            {
                return null;
            }

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
        public MaterialDTO findMaterialByID(long materialID)
        {
            return PersistenceContext.repositories().createMaterialRepository().find(materialID).toDTO();
        }
        public MaterialDTO findMaterialByReference(string reference)
        {
            return PersistenceContext.repositories().createMaterialRepository().find(reference).toDTO();
        }

        /// <summary>
        /// Updates the Material on the MaterialRepository given its data.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>DTO that represents the updated Material</returns>
        //TODO: user might not want to update all aspects of the material, and as such the dto might be incomplete; check which fields are empty
        public MaterialDTO updateMaterial(MaterialDTO materialDTO)
        {
            MaterialRepository repository = PersistenceContext.repositories().createMaterialRepository();
            Material material = repository.find(materialDTO.id);

            if (material == null)
            {
                return null;
            }

            material.changeReference(materialDTO.reference);
            material.changeDesignation(materialDTO.designation);

            foreach (ColorDTO colorDTO in materialDTO.colors)
            {
                string name = colorDTO.name;
                byte red = colorDTO.red;
                byte green = colorDTO.green;
                byte blue = colorDTO.blue;
                byte alpha = colorDTO.alpha;
                material.addColor(Color.valueOf(name, red, green, blue, alpha));
            }

            foreach (FinishDTO finishDTO in materialDTO.finishes)
            {
                material.addFinish(Finish.valueOf(finishDTO.description));
            }
            Material mat = repository.update(material);
            return mat == null ? null : mat.toDTO();
        }
        /// Parses an enumerable of materials persistence identifiers as an enumerable of entities
        /// </summary>
        /// <param name="materialsIDS">Enumerable with the materials persistence identifiers</param>
        /// <returns>IEnumerable with the materials ids as entities</returns>
        internal IEnumerable<Material> enumerableOfMaterialsIDSAsEntities(IEnumerable<long> materialsIDS)
        {
            if (materialsIDS == null) return null;
            List<Material> materials = new List<Material>();
            IEnumerator<long> materialsIDSEnumerator = materialsIDS.GetEnumerator();
            long nextMaterialID = materialsIDSEnumerator.Current;
            while (materialsIDSEnumerator.MoveNext())
            {
                nextMaterialID = materialsIDSEnumerator.Current;
                //Uncomment when Material Persistence ID is changed to long
                //materials.Add(materialRepository.find(nextMaterialID));
            }
            return materials;
        }
        /// <summary>
        /// Updates the finishes of a material
        /// </summary>
        /// <param name="updateMaterialDTO">UpdateMaterialDTO with the data regarding the material update</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateFinishes(UpdateMaterialDTO updateMaterialDTO)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(updateMaterialDTO.id);

            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;

            if (updateMaterialDTO.finishesToAdd != null)
            {
                foreach (FinishDTO finish in updateMaterialDTO.finishesToAdd)
                {
                    updatedWithSuccess &= material.addFinish(finish.toEntity());
                    perfomedAtLeastOneUpdate = true;
                }
            }
            if (updateMaterialDTO.finishesToRemove != null)
            {
                foreach (FinishDTO finish in updateMaterialDTO.finishesToRemove)
                {
                    updatedWithSuccess &= material.removeFinish(finish.toEntity());
                    perfomedAtLeastOneUpdate = true;
                }
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= materialRepository.update(material) != null;
            return updatedWithSuccess;
        }
        /// <summary>
        /// Updates the colors of a material
        /// </summary>
        /// <param name="updateMaterialDTO">UpdateMaterialDTO with the data regarding the material update</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool updateColors(UpdateMaterialDTO updateMaterialDTO)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(updateMaterialDTO.id);

            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;

            if (updateMaterialDTO.colorsToAdd != null)
            {
                foreach (ColorDTO color in updateMaterialDTO.colorsToAdd)
                {
                    updatedWithSuccess &= material.addColor(color.toEntity());
                    perfomedAtLeastOneUpdate = true;
                }
            }
            if (updateMaterialDTO.colorsToRemove != null)
            {
                foreach (ColorDTO color in updateMaterialDTO.colorsToRemove)
                {
                    updatedWithSuccess &= material.removeColor(color.toEntity());
                    perfomedAtLeastOneUpdate = true;
                }
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= materialRepository.update(material) != null;
            return updatedWithSuccess;
        }
    }
}