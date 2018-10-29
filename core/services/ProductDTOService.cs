using core.domain;
using core.dto;
using core.persistence;
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
        /// Constant that represents the message that occures if the materials being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_MATERIALS_FETCH = "The materials being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occures if the components being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_COMPONENTS_FETCH = "The components being fetched are invalid";

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
                ensureProductsComponentsFetchWasSuccesful(productDTO.complements,productComplementedProducts);
            }
            ProductCategory productCategory=PersistenceContext.repositories().createProductCategoryRepository().find(productDTO.productCategory.id);
            IEnumerable<Material> productMaterials=PersistenceContext.repositories().createMaterialRepository().getMaterialsByIDS(productDTO.productMaterials);
            ensureMaterialsFetchWasSuccessful(productDTO.productMaterials,productMaterials);

            IEnumerable<Dimension> productHeightDimensions=DTOUtils.reverseDTOS(productDTO.dimensions.heightDimensionDTOs);
            IEnumerable<Dimension> productWidthDimensions=DTOUtils.reverseDTOS(productDTO.dimensions.widthDimensionDTOs);
            IEnumerable<Dimension> productDepthDimensions=DTOUtils.reverseDTOS(productDTO.dimensions.depthDimensionDTOs);
            SlotDimensionSetDTO slotDimensions=null;
            if(productDTO.slotDimensions!=null)
                slotDimensions=productDTO.slotDimensions;
            if(slotDimensions != null && productComplementedProducts != null){
                CustomizedDimensions maxSlotDimension=slotDimensions.maximumSlotDimensions.toEntity();
                CustomizedDimensions minSlotDimension=slotDimensions.minimumSlotDimensions.toEntity();
                CustomizedDimensions recommendedSlotDimension = slotDimensions.recommendedSlotDimensions.toEntity();

                return new Product(reference, designation,true,
                                            maxSlotDimension,
                                            minSlotDimension,
                                            recommendedSlotDimension,
                                            productCategory,
                                            productMaterials,
                                            productComplementedProducts,
                                            productHeightDimensions,
                                            productWidthDimensions,
                                            productDepthDimensions);     
            }   
            if(productComplementedProducts==null&&slotDimensions!=null){
                CustomizedDimensions maxSlotDimension=slotDimensions.maximumSlotDimensions.toEntity();
                CustomizedDimensions minSlotDimension=slotDimensions.minimumSlotDimensions.toEntity();
                CustomizedDimensions recommendedSlotDimension = slotDimensions.recommendedSlotDimensions.toEntity();

                return new Product(reference,designation,true,
                                            maxSlotDimension,
                                            minSlotDimension,
                                            recommendedSlotDimension,
                                            productCategory, 
                                            productMaterials,
                                            productHeightDimensions,
                                            productWidthDimensions,
                                            productDepthDimensions);
            }
            if(productComplementedProducts!=null&&slotDimensions==null){
                return new Product(reference,designation,
                                            productCategory, 
                                            productMaterials,
                                            productComplementedProducts,
                                            productHeightDimensions,
                                            productWidthDimensions,
                                            productDepthDimensions);
            }
            return new Product(reference,designation,productCategory
                                            ,productMaterials
                                            ,productHeightDimensions
                                            ,productWidthDimensions
                                            ,productDepthDimensions); 
        }
        
        /// <summary>
        /// Ensures that the materials fetch was successful
        /// </summary>
        /// <param name="materialsToFetch">IEnumerable with the materials dtos to fetch</param>
        /// <param name="fetchedMaterials">IEnumerable with the fetched materials</param>
        private void ensureMaterialsFetchWasSuccessful(IEnumerable<MaterialDTO> materialsToFetch, IEnumerable<Material> fetchedMaterials) {
            if (Collections.isEnumerableNullOrEmpty(materialsToFetch) || Collections.getEnumerableSize(materialsToFetch) != Collections.getEnumerableSize(fetchedMaterials))
                throw new InvalidOperationException(INVALID_MATERIALS_FETCH);
        }

        /// <summary>
        /// Ensures that the produts components fetch was successful
        /// </summary>
        /// <param name="componentsToFetch">IEnumerable with the components dtos to fetch</param>
        /// <param name="fetchedComponents">IEnumerable with the fetched components</param>
        private void ensureProductsComponentsFetchWasSuccesful(IEnumerable<ComponentDTO> componentsToFetch, IEnumerable<Product> fetchedComponents) {
            if (Collections.isEnumerableNullOrEmpty(componentsToFetch) || Collections.getEnumerableSize(componentsToFetch) != Collections.getEnumerableSize(fetchedComponents))
                throw new InvalidOperationException(INVALID_COMPONENTS_FETCH);
        }
    }
}