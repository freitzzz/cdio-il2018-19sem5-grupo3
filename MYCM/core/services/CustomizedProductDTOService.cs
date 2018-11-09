using core.domain;
using core.dto;
using core.persistence;
using support.dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.services
{

    /// <summary>
    /// Service class that helps the transformation of CustomizedProductDTO into CustomizedProduct since some information needs to be accessed on the persistence context
    /// </summary>
    public sealed class CustomizedProductDTOService
    {

        /// <summary>
        /// Private constructor used for hiding the implicit public one.
        /// </summary>
        private CustomizedProductDTOService() { }

        /// <summary>
        /// Transforms a customized product dto into a customized product via service
        /// </summary>
        /// <param name="customizedProductDTO">CustomizedProductDTO with the customized product dto being transformed</param>
        /// <returns>CustomizedProduct with the of customized products transformed from the dto</returns>
        public static CustomizedProduct transform(CustomizedProductDTO customizedProductDTO)
        {
            CustomizedProduct customizedProduct = null;

            string reference = customizedProductDTO.reference;
            string designation = customizedProductDTO.designation;

            //Fetch the product associated to this customized product by its id
            long productId = customizedProductDTO.productDTO.id;
            Product product = PersistenceContext.repositories().createProductRepository().find(productId);

            //Fetch the material associated to the customized material
            long materialId = customizedProductDTO.customizedMaterialDTO.material.id;
            Material material = PersistenceContext.repositories().createMaterialRepository().find(materialId);

            Finish customizedFinish = customizedProductDTO.customizedMaterialDTO.finish.toEntity();
            Color customizedColor = customizedProductDTO.customizedMaterialDTO.color.toEntity();

            CustomizedDimensions customizedDimensions = customizedProductDTO.customizedDimensionsDTO.toEntity();
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, customizedColor, customizedFinish);

            //check if the dto contains slot information
            if (customizedProductDTO.slotListDTO == null)
            {
                customizedProduct = new CustomizedProduct(reference, designation, customizedMaterial, customizedDimensions, product);
            }
            else
            {
                //if the dto contains slot info, then create one with slots
                List<Slot> slots = new SlotDTOService().transform(customizedProductDTO.slotListDTO).ToList();
                customizedProduct = new CustomizedProduct(reference, designation, customizedMaterial, customizedDimensions, product, slots);
            }

            return customizedProduct;
        }
    }
}