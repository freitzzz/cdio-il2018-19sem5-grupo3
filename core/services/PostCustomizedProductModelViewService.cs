using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using core.persistence;
using support.dto;

namespace core.services
{
    /// <summary>
    /// Service that helps transforming a PostCustomizedProductModelViewService into a CustomizedProduct
    /// </summary>
    public static class PostCustomizedProductModelViewService
    {
        /// <summary>
        /// Transforms a PostCustomizedProductModelView into a CustomizedProduct
        /// </summary>
        /// <param name="customizedProductModelView">model view with the customized products info</param>
        /// <returns>CustomizedProduct instance</returns>
        public static CustomizedProduct transform(PostCustomizedProductModelView customizedProductModelView)
        {

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

            if (customizedProductModelView.slots == null)
            {
                return new CustomizedProduct(
                    reference, designation,
                    customizedMaterial, customizedDimensions,
                    product
                );
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

                return new CustomizedProduct(
                    reference, designation,
                    customizedMaterial, customizedDimensions,
                    product, slots
                );
            }
        }

        /// <summary>
        /// Transforms a CustomizedProduct into a PostCustomizedProductModelView
        /// </summary>
        /// <param name="customizedProduct">CustomizedProduct to be transformed</param>
        /// <returns>PostCustomizedProductModelView</returns>
        public static PostCustomizedProductModelView transform(CustomizedProduct customizedProduct)
        {
            PostCustomizedProductModelView customizedProductModelView = new PostCustomizedProductModelView();

            customizedProductModelView.id = customizedProduct.Id;
            customizedProductModelView.reference = customizedProduct.reference;
            customizedProductModelView.designation = customizedProduct.designation;
            customizedProductModelView.productId = customizedProduct.product.Id;
            customizedProductModelView.customizedMaterialDTO = customizedProduct.customizedMaterial.toDTO();
            customizedProductModelView.customizedDimensionsDTO = customizedProduct.customizedDimensions.toDTO();

            List<CustomizedDimensions> slotDimensionsList = new List<CustomizedDimensions>();
            if (customizedProduct.slots != null)
            {
                foreach (Slot slot in customizedProduct.slots)
                {
                    slotDimensionsList.Add(slot.slotDimensions);
                }

                customizedProductModelView.slots = DTOUtils.parseToDTOS(slotDimensionsList).ToList();
                return customizedProductModelView;
            }
            else
            {
                customizedProductModelView.slots = new List<CustomizedDimensionsDTO>();
                return customizedProductModelView;
            }
        }
    }
}