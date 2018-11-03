using System;
using core.domain;
using core.dto;
using core.persistence;

namespace core.services
{
    /// <summary>
    /// Service to help update a customized product
    /// </summary>
    public static class UpdateCustomizedProductModelViewService
    {
        /// <summary>
        /// Updates a CustomizedProduct with the info that's on an UpdateCustomizedProductModelView
        /// </summary>
        /// <param name="updateCustomizedProductModelView">ModelView containing the update info of the customized product</param>
        /// <returns>true if the customized product was updated successfully, false if otherwise</returns>
        public static bool update(UpdateCustomizedProductModelView updateCustomizedProductModelView)
        {
            CustomizedProductRepository customizedProductRepository =
                PersistenceContext.repositories().
                        createCustomizedProductRepository();

            CustomizedProduct customizedProductBeingUpdated = customizedProductRepository.find(updateCustomizedProductModelView.Id);
            bool updatedWithSuccess = true;
            bool performedAtLeastOneUpdate = false;

            if (updateCustomizedProductModelView.reference != null)
            {
                updatedWithSuccess &= customizedProductBeingUpdated.changeReference(updateCustomizedProductModelView.reference);
                performedAtLeastOneUpdate = true;
            }

            if (updateCustomizedProductModelView.designation != null)
            {
                updatedWithSuccess &= customizedProductBeingUpdated.changeDesignation(updateCustomizedProductModelView.designation);
                performedAtLeastOneUpdate = true;
            }

            if (updateCustomizedProductModelView.customizedDimensions != null)
            {
                CustomizedDimensions updatedCustomizedDimensions = updateCustomizedProductModelView.customizedDimensions.toEntity();
                updatedWithSuccess &= customizedProductBeingUpdated.changeCustomizedDimensions(updatedCustomizedDimensions);
                performedAtLeastOneUpdate = true;
            }

            if (updateCustomizedProductModelView.customizedMaterial != null)
            {
                MaterialRepository materialRepository = PersistenceContext.repositories().createMaterialRepository();
                Material materialFromUpdate = materialRepository.find(updateCustomizedProductModelView.customizedMaterial.material.id);
                Material currentMaterial = materialRepository.find(customizedProductBeingUpdated.customizedMaterial.material.Id);
                Finish updatedFinish = null;
                Color updatedColor = null;
                bool newMaterialReference = true;
                bool colorUpdated = false;
                bool finishUpdated = false;

                //Check if there's a new material reference to know
                //if the customized material has to change as a whole
                //or only if it's properties need changing
                if (materialFromUpdate.Equals(currentMaterial))
                {
                    newMaterialReference = false;
                }

                if (updateCustomizedProductModelView.customizedMaterial.finish != null)
                {
                    updatedFinish = updateCustomizedProductModelView.customizedMaterial.finish.toEntity();
                }

                if (updateCustomizedProductModelView.customizedMaterial.color != null)
                {
                    updatedColor = updateCustomizedProductModelView.customizedMaterial.color.toEntity();
                }

                if (updatedFinish == null && updatedColor != null)
                {
                    updatedWithSuccess &= customizedProductBeingUpdated.changeColor(updatedColor);
                    performedAtLeastOneUpdate = true;
                    colorUpdated = true;
                }

                if (updatedFinish != null && updatedColor == null)
                {
                    updatedWithSuccess &= customizedProductBeingUpdated.changeFinish(updatedFinish);
                    performedAtLeastOneUpdate = true;
                    finishUpdated = true;
                }

                if (updatedFinish != null && updatedColor != null)
                {
                    //If there's a new material reference
                    //there's a need to create a new CustomizedMaterial object
                    //otherwise we only need to change the color/finish properties of the
                    //customized material
                    if (newMaterialReference)
                    {
                        CustomizedMaterial newCustomizedMaterial = CustomizedMaterial.valueOf(materialFromUpdate, updatedColor, updatedFinish);
                        updatedWithSuccess &= customizedProductBeingUpdated.changeCustomizedMaterial(newCustomizedMaterial);
                        performedAtLeastOneUpdate = true;
                    }
                    else if (!colorUpdated && !finishUpdated)
                    {
                        updatedWithSuccess &= customizedProductBeingUpdated.changeColor(updatedColor);
                        updatedWithSuccess &= customizedProductBeingUpdated.changeFinish(updatedFinish);
                        performedAtLeastOneUpdate = true;
                    }
                }
            }

            if (!performedAtLeastOneUpdate || !updatedWithSuccess)
            {
                return false;
            }
            updatedWithSuccess &= customizedProductRepository.update(customizedProductBeingUpdated) != null;
            return updatedWithSuccess;
        }
    }
}