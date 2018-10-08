using core.domain;
using core.dto;
using core.persistence;
using support.dto;
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
            IEnumerable<Component> productComponents=PersistenceContext.repositories().createComponentRepository().fetchComponentsByIDS(productDTO.complements);
            ProductCategory productCategory=PersistenceContext.repositories().createProductCategoryRepository().find(productDTO.productCategory.id);
            //TODO:Check if the length of product materials is equal to product materials dtos
            IEnumerable<Material> productMaterials=PersistenceContext.repositories().createMaterialRepository().getMaterialsByIDS(productDTO.productMaterials);
            //TODO:Currently DTO Mapping for dimensions isn't working
            IEnumerable<Dimension> productHeightDimensions=DTOUtils.reverseDTOS(productDTO.dimensions.heightDimensionDTOs);
            IEnumerable<Dimension> productWidthDimensions=DTOUtils.reverseDTOS(productDTO.dimensions.widthDimensionDTOs);
            IEnumerable<Dimension> productDepthDimensions=DTOUtils.reverseDTOS(productDTO.dimensions.depthDimensionDTOs);
            if(productComponents==null)
                return new Product(reference,designation,productCategory
                                            ,productMaterials
                                            ,productHeightDimensions
                                            ,productWidthDimensions
                                            ,productDepthDimensions);
            return new Product(reference,designation,productCategory
                                            ,productMaterials
                                            ,productComponents
                                            ,productHeightDimensions
                                            ,productWidthDimensions
                                            ,productDepthDimensions); 
        }
    }
}