using System;
using core.domain;
using core.dto;
using core.modelview.customizedmaterial;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Static class representing the Service used for creating instances of CustomizedMaterial.
    /// </summary>
    public static class CreateCustomizedMaterialService
    {
        /// <summary>
        /// Constant representing the error message presented when the Material is not found.
        /// </summary>
        private const string ERROR_UNABLE_TO_FIND_MATERIAL = "Unable to find a material with an identifier of: {0}";

        /// <summary>
        /// Constant representing the error message presented when the CustomizedProduct's customized material has null color and finish
        /// </summary>
        private const string NULL_COLOR_AND_FINISH = "Color and Finish of customized material both null";

        /// <summary>
        /// Creates a new instance of CustomizedMaterial.
        /// </summary>
        /// <param name="addCustomizedMaterialModelView">AddCustomizedMaterialModelView representing the CustomizedMaterial's data.</param>
        /// <returns>An instance of CustomizedMaterial.</returns>
        /// <exception cref="System.ArgumentException">Thrown when no Material could be found with the provided identifier.</exception>
        public static CustomizedMaterial create(AddCustomizedMaterialModelView addCustomizedMaterialModelView)
        {
            MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();

            Material material = materialRepository.find(addCustomizedMaterialModelView.materialId);

            if (material == null)
            {
                throw new ArgumentException(string.Format(ERROR_UNABLE_TO_FIND_MATERIAL, addCustomizedMaterialModelView.materialId));
            }
            //TODO: replace usage of dto
            FinishDTO finishDTO = addCustomizedMaterialModelView.finish;
            ColorDTO colorDTO = addCustomizedMaterialModelView.color;

            if (finishDTO == null && colorDTO == null)
            {
                throw new ArgumentException(NULL_COLOR_AND_FINISH);
            }

            CustomizedMaterial customizedMaterial = null;

            if (finishDTO == null && colorDTO != null)
            {
                customizedMaterial = CustomizedMaterial.valueOf(material, colorDTO.toEntity());
            }
            else if (finishDTO != null && colorDTO == null)
            {
                customizedMaterial = CustomizedMaterial.valueOf(material, finishDTO.toEntity());
            }
            else
            {
                customizedMaterial = CustomizedMaterial.valueOf(material, colorDTO.toEntity(), finishDTO.toEntity());
            }

            return customizedMaterial;
        }
    }
}