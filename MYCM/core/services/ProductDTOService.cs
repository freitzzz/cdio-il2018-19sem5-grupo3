using core.domain;
using core.dto;
using core.persistence;
using core.services.ensurance;
using support.dto;
using support.utils;
using System.Collections.Generic;
using System;

namespace core.services{
    /// <summary>
    /// Service class that helps the transformation of ProductDTO into Product since some information needs to be accessed on the persistence context
    /// </summary>
    public sealed class ProductDTOService{

        /// <summary>
        /// Transforms a product dto into a product via service
        /// </summary>
        /// <param name="productDTO">ProductDTO with the product dto being transformed</param>
        /// <returns>Product with the product transformed from the dto</returns>
        public Product transform(ProductDTO productDTO){
            string reference=productDTO.reference;
            string designation=productDTO.designation;
            IEnumerable<Product> productComplementedProducts=null;
            if(productDTO.complements!=null){
                productComplementedProducts=new ComponentDTOService().transform(productDTO.complements);
                FetchEnsurance.ensureProductsComponentsFetchWasSuccesful(productDTO.complements,productComplementedProducts);
            }

            ProductCategory productCategory=PersistenceContext.repositories().createProductCategoryRepository().find(productDTO.productCategory.id);
            ProductCategoryEnsurance.ensureProductCategoryIsLeaf(productCategory);

            IEnumerable<Material> productMaterials=PersistenceContext.repositories().createMaterialRepository().getMaterialsByIDS(productDTO.productMaterials);
            FetchEnsurance.ensureMaterialsFetchWasSuccessful(productDTO.productMaterials,productMaterials);

            IEnumerable<Measurement> productMeasurements = DTOUtils.reverseDTOS(productDTO.dimensions);

            SlotDimensionSetDTO slotDimensions=null;
            if(productDTO.slotDimensions!=null)
                slotDimensions=productDTO.slotDimensions;
            if(slotDimensions != null && productComplementedProducts != null){
                CustomizedDimensions maxSlotDimension=slotDimensions.maximumSlotDimensions.toEntity();
                CustomizedDimensions minSlotDimension=slotDimensions.minimumSlotDimensions.toEntity();
                CustomizedDimensions recommendedSlotDimension = slotDimensions.recommendedSlotDimensions.toEntity();

                return new Product(reference, designation,
                                            maxSlotDimension,
                                            minSlotDimension,
                                            recommendedSlotDimension,
                                            productCategory,
                                            productMaterials,
                                            productComplementedProducts,
                                            productMeasurements);     
            }   
            if(productComplementedProducts==null&&slotDimensions!=null){
                CustomizedDimensions maxSlotDimension=slotDimensions.maximumSlotDimensions.toEntity();
                CustomizedDimensions minSlotDimension=slotDimensions.minimumSlotDimensions.toEntity();
                CustomizedDimensions recommendedSlotDimension = slotDimensions.recommendedSlotDimensions.toEntity();

                return new Product(reference,designation,
                                            maxSlotDimension,
                                            minSlotDimension,
                                            recommendedSlotDimension,
                                            productCategory, 
                                            productMaterials,
                                            productMeasurements);
            }
            if(productComplementedProducts!=null&&slotDimensions==null){
                return new Product(reference,designation,
                                            productCategory, 
                                            productMaterials,
                                            productComplementedProducts,
                                            productMeasurements);
            }
            return new Product(reference,designation,productCategory
                                            ,productMaterials
                                            ,productMeasurements); 
        }
    }
}