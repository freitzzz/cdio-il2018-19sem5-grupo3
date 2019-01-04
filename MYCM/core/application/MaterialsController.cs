using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;
using core.dto;
using core.services;
using core.modelview.material;
using core.exceptions;

namespace core.application
{

    /// <summary>
    /// Core MaterialsController class.
    /// </summary>
    public class MaterialsController
    {
        /// <summary>
        /// Constant representing the message presented when the requested material isn't found
        /// </summary>
        private const string ERROR_MATERIAL_NOT_FOUND = "Unable to find a material with an identifier of: {0}";

        /// <summary>
        /// Constant representing the message presented when no finishes have a current price
        /// </summary>
        private const string NO_PRICED_FINISHES = "The requested material doesn't have any currently priced finishes";

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
            if (updateMaterialDTO.image != null)
            {
                updatedWithSuccess &= materialBeingUpdated.changeImage(updateMaterialDTO.image);
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
        public void disableMaterial(MaterialDTO materialDTO)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material materialBeingDisabled = materialRepository.find(materialDTO.id);

            if (materialBeingDisabled == null)
            {
                throw new ResourceNotFoundException(string.Format(ERROR_MATERIAL_NOT_FOUND, materialDTO.id));
            }

            materialRepository.remove(materialBeingDisabled);
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
        public MaterialDTO findMaterialByID(long materialID, bool pricedFinishesOnly)
        {
            Material material = PersistenceContext.repositories().createMaterialRepository().find(materialID);

            if (material == null)
            {
                throw new ResourceNotFoundException(ERROR_MATERIAL_NOT_FOUND);
            }

            if (pricedFinishesOnly)
            {
                FinishPriceTableRepository finishPriceTableRepository =
                    PersistenceContext.repositories().createFinishPriceTableRepository();
                List<Finish> pricedFinishes = new List<Finish>();
                foreach (Finish finish in material.Finishes)
                {
                    if (finishPriceTableRepository.fetchCurrentMaterialFinishPrice(finish.Id) != null)
                    {
                        pricedFinishes.Add(finish);
                    }
                }

                if (pricedFinishes.Count == 0)
                {
                    throw new ResourceNotFoundException(NO_PRICED_FINISHES);
                }

                material.Finishes.Clear();
                material.Finishes.AddRange(pricedFinishes);
                return material.toDTO();
            }

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
            material.changeImage(materialDTO.image);

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
                material.addFinish(Finish.valueOf(finishDTO.description, finishDTO.shininess));
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
        /// Add the finish of a material from update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="addFinishDTO">FinishDTO with the information of finish to add</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public AddFinishModelView addFinish(long idMaterial, FinishDTO addFinishDTO)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(idMaterial);
            if (addFinishDTO != null)
            {
                material.addFinish(addFinishDTO.toEntity());


                materialRepository.update(material);
                AddFinishModelView addFinishModelView = new AddFinishModelView();
                addFinishModelView.finish = addFinishDTO;
                return addFinishModelView;
            }
            return null;
        }
        /// <summary>
        /// Remove the Finish of a material from update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="idFinish">id of the finish to be remove</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool removeFinish(long idMaterial, long idFinsih)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(idMaterial);

            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;

            material = materialRepository.deleteFinish(idMaterial, idFinsih);
            updatedWithSuccess = true;
            perfomedAtLeastOneUpdate = true;

            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= materialRepository.update(material) != null;
            return updatedWithSuccess;
        }
        /// <summary>
        /// Add the color of a material from update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="addColorDTO">ColorDTO with the information of color to add</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public AddColorModelView addColor(long idMaterial, ColorDTO addColorDTO)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(idMaterial);
            if (addColorDTO != null)
            {
                Color colorToAdd = addColorDTO.toEntity();
                material.addColor(colorToAdd);


                materialRepository.update(material);
                AddColorModelView addColorModelView = new AddColorModelView();
                addColorModelView.color = addColorDTO;
                return addColorModelView;
            }
            return null;
        }
        /// <summary>
        /// Remove the color of a material from update
        /// </summary>
        /// <param name="idMaterial">id of the material to be updated</param>
        /// <param name="idColor">id of the color to be remove</param>
        /// <returns>boolean true if the update was successful, false if not</returns>
        public bool removeColor(long idMaterial, long idColor)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(idMaterial);

            bool updatedWithSuccess = true;
            bool perfomedAtLeastOneUpdate = false;

            material = materialRepository.deleteColor(idMaterial, idColor);
            updatedWithSuccess = true;
            perfomedAtLeastOneUpdate = true;

            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess &= materialRepository.update(material) != null;
            return updatedWithSuccess;
        }
    }
}