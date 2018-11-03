using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using core.modelview.customizedproduct;
using core.persistence;
using support.dto;

namespace core.services
{  
    /// <summary>
    /// Service to help transform a PostCustomizedProductToSlotModelView into a customized product
    /// </summary>
    public static class PostCustomizedProductToSlotModelViewService
    {
        /// <summary>
        /// Transforms a PostCustomizedProductToSlotModelView into a CustomizedProduct
        /// </summary>
        /// <param name="customizedProductModelView">ModelView of the customized product being created and added to a slot</param>
        /// <returns>newly created CustomizedProduct</returns>
        public static CustomizedProduct transform(PostCustomizedProductToSlotModelView customizedProductModelView)
        {

            CustomizedProductRepository customizedProductRepository = PersistenceContext.
                repositories().
                    createCustomizedProductRepository();

            //TODO Replace foreach with query
            //!This find method is causing the following
            //!An exception occurred in the database while iterating the results of a query for context type 'backend.persistence.ef.MyCContext'.
            //!System.InvalidOperationException: A second operation started on this context before a previous operation completed. Any instance members are not guaranteed to be thread safe.
            /* Slot slot = customizedProductRepository.
                        findSlot(
                                customizedProductModelView.baseId,
                                customizedProductModelView.slotId
                        ); */
            CustomizedProduct baseProduct = customizedProductRepository.find(customizedProductModelView.baseId);

            //!Temporary solution using foreach because of what's mentioned above
            Slot slot = null;

            foreach(Slot baseSlot in baseProduct.slots){
                if(baseSlot.Id == customizedProductModelView.slotId){
                    slot = baseSlot;
                    break;
                }
            }

            if(slot == null){
                return null;
            }

            string reference = customizedProductModelView.reference;
            string designation = customizedProductModelView.designation;

            long productId = customizedProductModelView.productId;
            Product product = PersistenceContext.repositories().createProductRepository().find(productId);

            long materialId = customizedProductModelView.customizedMaterialDTO.material.id;
            Material material = PersistenceContext.repositories().createMaterialRepository().find(materialId);

            Finish customizedFinish = customizedProductModelView.customizedMaterialDTO.finish.toEntity();
            Color customizedColor = customizedProductModelView.customizedMaterialDTO.color.toEntity();

            CustomizedDimensions customizedDimensions = customizedProductModelView.customizedDimensionsDTO.toEntity();
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, customizedColor, customizedFinish);

            CustomizedProduct customizedProductToAddToSlot;

            if (customizedProductModelView.slots == null)
            {
                customizedProductToAddToSlot = new CustomizedProduct(
                    reference, designation,
                    customizedMaterial, customizedDimensions,
                    product, slot);
            }
            else
            {
                List<CustomizedDimensions> slotDimensionsList = DTOUtils.
                   reverseDTOS(customizedProductModelView.slots).ToList();

                List<Slot> slots = new List<Slot>();

                foreach (CustomizedDimensions slotDimensions in slotDimensionsList)
                {
                    slots.Add(new Slot(slotDimensions));
                }

                customizedProductToAddToSlot = new CustomizedProduct(
                    reference, designation,
                    customizedMaterial, customizedDimensions,
                    product, slots, slot);
            }

            return customizedProductToAddToSlot;
        }
    }
}