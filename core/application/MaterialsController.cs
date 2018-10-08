using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;
using core.dto;

namespace core.application {

    /// <summary>
    /// Core MaterialsController class.
    /// </summary>
    public class MaterialsController {

        /// <summary>
        /// Builds a new MaterialsController
        /// </summary>
        public MaterialsController() { }

        /// <summary>
        /// Fetches a List with all Materials present in the MaterialRepository.
        /// </summary>
        /// <returns>a List with all of the Material's DTOs</returns>
        public List<MaterialDTO> findAllMaterials() {
            List<MaterialDTO> dtoMaterials = new List<MaterialDTO>();
            IEnumerable<Material> materials = PersistenceContext.repositories().createMaterialRepository().findAll();

            foreach (Material material in materials) {
                dtoMaterials.Add(material.toDTO());
            }

            return dtoMaterials;
        }

        /// <summary>
        /// Fetches a Material from the MaterialRepository given its ID.
        /// </summary>
        /// <param name = "materialID">the Material's ID</param>
        /// <returns>DTO that represents the Material</returns>
        public MaterialDTO findMaterialByID(long materialID) {
            return PersistenceContext.repositories().createMaterialRepository().find(materialID).toDTO();
        }

        /// <summary>
        /// Removes a Material from the MaterialRepository given its ID.
        /// </summary>
        /// <param name="materialID">the Material's ID</param>
        /// <returns>DTO that represents the Material</returns>
        public MaterialDTO removeMaterial(long materialID) {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(materialID);
            return materialRepository.remove(material).toDTO();
        }

        /// <summary>
        /// Adds a Material to the MaterialRepository given its ID.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>DTO that represents the Material</returns>
        public MaterialDTO addMaterial(MaterialDTO materialAsDTO) {
            string reference = materialAsDTO.reference;
            string designation = materialAsDTO.designation;

            List<Color> colors = new List<Color>();
            foreach (ColorDTO colorDTO in materialAsDTO.colors) {
                string name = colorDTO.name;
                byte red = colorDTO.red;
                byte green = colorDTO.green;
                byte blue = colorDTO.blue;
                byte alpha = colorDTO.alpha;
                colors.Add(Color.valueOf(name, red, green, blue, alpha));
            }

            List<Finish> finishes = new List<Finish>();
            foreach (FinishDTO finishDTO in materialAsDTO.finishes) {
                finishes.Add(Finish.valueOf(finishDTO.description));
            }

            Material addedMaterial = PersistenceContext.repositories().createMaterialRepository().save(new Material(reference, designation, colors, finishes));

            return addedMaterial == null ? null : addedMaterial.toDTO();
        }

        /// <summary>
        /// Updates the Material on the MaterialRepository given its data.
        /// </summary>
        /// <param name="materialDTO">DTO that holds all info about the Material</param>
        /// <returns>DTO that represents the updated Material</returns>
        //TODO: user might not want to update all aspects of the material, and as such the dto might be incomplete; check which fields are empty
        public MaterialDTO updateMaterial(MaterialDTO materialDTO) {
            MaterialRepository repository = PersistenceContext.repositories().createMaterialRepository();
            Material material = repository.find(materialDTO.id);

            if (material == null) {
                return null;
            }

            material.changeReference(materialDTO.reference);
            material.changeDesignation(materialDTO.designation);

            foreach (ColorDTO colorDTO in materialDTO.colors) {
                string name = colorDTO.name;
                byte red = colorDTO.red;
                byte green = colorDTO.green;
                byte blue = colorDTO.blue;
                byte alpha = colorDTO.alpha;
                material.addColor(Color.valueOf(name, red, green, blue, alpha));
            }

            foreach (FinishDTO finishDTO in materialDTO.finishes) {
                material.addFinish(Finish.valueOf(finishDTO.description));
            }
            Material mat = repository.update(material);
            return mat == null ? null : mat.toDTO();
        }

        /// Parses an enumerable of materials persistence identifiers as an enumerable of entities
        /// </summary>
        /// <param name="materialsIDS">Enumerable with the materials persistence identifiers</param>
        /// <returns>IEnumerable with the materials ids as entities</returns>
        internal IEnumerable<Material> enumerableOfMaterialsIDSAsEntities(IEnumerable<long> materialsIDS) {
            if (materialsIDS == null) return null;
            List<Material> materials = new List<Material>();
            IEnumerator<long> materialsIDSEnumerator = materialsIDS.GetEnumerator();
            long nextMaterialID = materialsIDSEnumerator.Current;
            while (materialsIDSEnumerator.MoveNext()) {
                nextMaterialID = materialsIDSEnumerator.Current;
                //Uncomment when Material Persistence ID is changed to long
                //materials.Add(materialRepository.find(nextMaterialID));
            }
            return materials;
        }
        /// <summary>
        /// Adds finishes to a material
        /// </summary>
        /// <param name="id">id of the material to update</param>
        /// <param name="finishes">list of finishes to be added</param>
        /// <returns>DTO of the updated material</returns>
        public MaterialDTO updateFinishes(long id, UpdateMaterialDTO upMat) {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
            Material material = materialRepository.find(id);
            List<Finish> finishList = new List<Finish>();
            if (upMat.finishesToAdd != null) {
                foreach (FinishDTO dto in upMat.finishesToAdd) {
                    material.addFinish(dto.toEntity());
                }
            }
            if (upMat.finishesToRemove != null) {
                foreach (FinishDTO dto in upMat.finishesToRemove) {
                    Finish fin = materialRepository.findFinish(material, dto.id);
                    material.removeFinish(fin);
                }
            }
            materialRepository.update(material);
            return material.toDTO();
        }
    }
}