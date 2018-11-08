using System;
using System.Collections.Generic;
using core.domain;
using core.persistence;
using core.dto;
using core.modelview.component;
using core.modelview.material;
using core.modelview.product;
using core.modelview.restriction;
using core.services;
using support.dto;
using support.utils;
using core.services.ensurance;

namespace core.application
{
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController{

        /// <summary>
        /// Constant that represents the message that occurs if the update of a product wasn't successful
        /// </summary>
        public const string INVALID_PRODUCT_UPDATE="An error occured while updating the product";
        /// <summary>
        /// Constant that represents the message that occurs if the user does not provide inputs
        /// </summary>
        private const string LIST_OF_INPUTS_MISSING = "The selected algorithm requires inputs!";
        /// <summary>
        /// Constant that represents the message that occures if the materials being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_MATERIALS_FETCH="The materials being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occures if the components being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_COMPONENTS_FETCH="The components being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occures if the products being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_PRODUCTS_FETCH="The products being fetched are invalid";
        /// <summary>
        /// Constant that represents the message that occures if the dimensions being fetched 
        /// are invalid
        /// </summary>
        private const string INVALID_DIMENSIONS_FETCH="The dimensions being fetched are invalid";

        /// <summary>
        /// Builds a new ProductController
        /// </summary>
        public ProductController(){ }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="addProductMV">ModelView with the product information</param>
        /// <returns>GetProductModelView with the created product, null if the product was not created</returns>
        public GetProductModelView addProduct(AddProductModelView addProductMV){
            Product newProduct= CreateProductService.create(addProductMV);
            newProduct=PersistenceContext.repositories().createProductRepository().save(newProduct);
            if (newProduct == null) return null;
            return ProductModelViewService.fromEntity(newProduct);
        }

        /// <summary>
        /// Updates the properties of a product
        /// </summary>
        /// <param name="updateProductPropertiesModelView">UpdateProductPropertiesModelView with the data regarding the product update</param>
        /// <returns>GetProductModelView with the updated product information</returns>
        public GetProductModelView updateProductProperties(UpdateProductPropertiesModelView updateProductPropertiesModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated=productRepository.find(updateProductPropertiesModelView.id);
            FetchEnsurance.ensureProductFetchWasSuccessful(productBeingUpdated);
            bool perfomedAtLeastOneUpdate=false;
            
            if(updateProductPropertiesModelView.reference!=null){
                productBeingUpdated.changeProductReference(updateProductPropertiesModelView.reference);
                perfomedAtLeastOneUpdate=true;
            }
            
            if(updateProductPropertiesModelView.designation!=null){
                productBeingUpdated.changeProductDesignation(updateProductPropertiesModelView.designation);
                perfomedAtLeastOneUpdate=true;
            }
            
            if(updateProductPropertiesModelView.categoryId!=0){
                ProductCategory categoryToUpdate=PersistenceContext.repositories().createProductCategoryRepository().find(updateProductPropertiesModelView.categoryId);
                FetchEnsurance.ensureProductCategoryFetchWasSuccessful(categoryToUpdate);
                productBeingUpdated.changeProductCategory(categoryToUpdate);
                perfomedAtLeastOneUpdate=true;
            }

            UpdateEnsurance.ensureAtLeastOneUpdateWasPerformed(perfomedAtLeastOneUpdate);

            productBeingUpdated=productRepository.update(productBeingUpdated);
            UpdateEnsurance.ensureProductUpdateWasSuccessful(productBeingUpdated);
            return ProductModelViewService.fromEntity(productBeingUpdated);
        }

        /// <summary>
        /// Updates the category of a Product.
        /// </summary>
        /// <param name="updateProductMV">UpdateProductModelView with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, false otherwise.</returns>
        private bool updateProductCategory(UpdateProductModelView updateProductMV){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated=productRepository.find(updateProductMV.productId);
            bool updatedWithSuccess=true;
            bool perfomedAtLeastOneUpdate=false;
            if (updateProductMV.productCategoryId.HasValue){
                ProductCategory productCategory=PersistenceContext.repositories().createProductCategoryRepository().find((updateProductMV.productCategoryId.Value));
                updatedWithSuccess&=productBeingUpdated.changeProductCategory(productCategory);
                perfomedAtLeastOneUpdate=true;
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;
            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Adds a material to a product
        /// </summary>
        /// <param name="addComponentToProductDTO">AddMaterialToProductDTO with the material addition information</param>
        /// <returns>MaterialDTO with the material that was added to the product</returns>
        public GetMaterialModelView addMaterialToProduct(AddMaterialToProductModelView addMaterialToProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToAddMaterial=productRepository.find(addMaterialToProductDTO.productID);
            //TODO:CHECK PRODUCT EXISTENCE
            Material materialBeingAdded=PersistenceContext.repositories().createMaterialRepository().find(addMaterialToProductDTO.materialID);
            //TODO:CHECK MATERIAL EXISTENCE
            productToAddMaterial.addMaterial(materialBeingAdded);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToAddMaterial);
            //TODO:REPLACE toDTO() WITH MODEL VIEW DTO (MaterialDetailsDTO)
            return MaterialModelViewService.fromEntity(materialBeingAdded);
        }

        /// <summary>
        /// Deletes a material from a product
        /// </summary>
        /// <param name="deleteMaterialFromProductDTO">DeleteMaterialFromProductDTO with the material deletion information</param>
        public void deleteMaterialFromProduct(DeleteMaterialFromProducModelView deleteMaterialFromProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToRemoveMaterial=productRepository.find(deleteMaterialFromProductDTO.productID);
            //TODO:CHECK PRODUCT EXISTENCE
            Material materialBeingDeleted=PersistenceContext.repositories().createMaterialRepository().find(deleteMaterialFromProductDTO.materialID);
            //TODO:CHECK MATERIAL EXISTENCE
            productToRemoveMaterial.removeMaterial(materialBeingDeleted);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToRemoveMaterial);
        }

        /// <summary>
        /// Adds a restriction to a product component material
        /// </summary>
        /// <param name="addRestrictionToProductMaterialDTO">AddRestrictionToProductComponentMaterialDTO with the restriction addition information</param>
        /// <returns>RestrictionDTO with the product component material added restriction</returns>
        public GetRestrictionModelView addRestrictionToProductComponentMaterial(AddRestrictionToProductComponentMaterialModelView addRestrictionToProductMaterialDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithMaterialBeingAddedRestriction=productRepository.find(addRestrictionToProductMaterialDTO.productID);
            //TODO: CHECK PRODUCT EXISTENCE
            Material productMaterialBeingAddedRestriction=PersistenceContext.repositories().createMaterialRepository().find(addRestrictionToProductMaterialDTO.materialID);
            //TODO: CHECK MATERIAL EXISTENCE
            //TODO: RESTRICTION DTO SERVICE
            
            productRepository.update(productWithMaterialBeingAddedRestriction);
            //TODO: CHECK PRODUCT UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a restriction from a product component material
        /// </summary>
        /// <param name="deleteRestrictionFromProductComponentMaterialDTO">DeleteRestrictionFromProductComponentMaterialDTO with the restriction deletion information</param>
        public void deleteRestrictionFromProductComponentMaterial(DeleteRestrictionFromProductComponentMaterialModelView deleteRestrictionFromProductComponentMaterialDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithMaterialBeingDeletedRestriction=productRepository.find(deleteRestrictionFromProductComponentMaterialDTO.productID);
            //TODO: CHECK PRODUCT EXISTENCE
            Material productMaterialBeingDeletedRestriction=PersistenceContext.repositories().createMaterialRepository().find(deleteRestrictionFromProductComponentMaterialDTO.materialID);
            //TODO: CHECK MATERIAL EXISTENCE
            //TODO: RESTRICTION REPOSITORY ? ? ? ? ? ? :\

            productRepository.update(productWithMaterialBeingDeletedRestriction);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a component to a product
        /// </summary>
        /// <param name="addComponentToProductDTO">AddComponentToProductDTO with the component addition information</param>
        /// <returns>ComponentDTO with the component that was added to the product</returns>
        public GetComponentModelView addComponentToProduct(AddComponentToProductModelView addComponentToProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToAddComponent=productRepository.find(addComponentToProductDTO.productID);
            //TODO:CHECK PRODUCT EXISTENCE
            Product componentBeingAdded=productRepository.find(addComponentToProductDTO.complementedProductID);
            //TODO:CHECK COMPLEMENTED PRODUCT EXISTENCE
            productToAddComponent.addComplementedProduct(componentBeingAdded);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToAddComponent);
            //TODO:REPLACE WITH MODEL VIEW DTO (ComponentDetailsDTO)
            //return componentBeingAdded.toDTO();
            return null;
        }
        
        /// <summary>
        /// Deletes a component from a product
        /// </summary>
        /// <param name="deleteComponentFromProductDTO">DeleteComponentFromProductDTO with the component deletion information</param>
        public void deleteComponentFromProduct(DeleteComponentFromProductModelView deleteComponentFromProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToRemoveComponent=productRepository.find(deleteComponentFromProductDTO.productID);
            //TODO:CHECK PRODUCT EXISTENCE
            Product productBeingDeleted=productRepository.find(deleteComponentFromProductDTO.componentID);
            //TODO:CHECK COMPLEMENTED PRODUCT EXISTENCE
            productToRemoveComponent.removeComplementedProduct(productBeingDeleted);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToRemoveComponent);
        }

        /// <summary>
        /// Adds a restriction to a product component
        /// </summary>
        /// <param name="addRestrictionToProductComponentDTO">AddRestrictionToProductComponentDTO with the restriction addition information</param>
        /// <returns>RestrictionDTO with the product component added restriction</returns>
        public GetRestrictionModelView addRestrictionToProductComponent(AddRestrictionToProductComponentModelView addRestrictionToProductComponentDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithComponentBeingAddedRestriction=productRepository.find(addRestrictionToProductComponentDTO.productID);
            //TODO: CHECK PRODUCT EXISTENCE
            Product productComponentBeingAddedRestriction=productRepository.find(addRestrictionToProductComponentDTO.componentID);
            //TODO: CHECK COMPLEMENTED PRODUCT EXISTENCE
            //TODO: RESTRICTION DTO SERVICE

            productRepository.update(productWithComponentBeingAddedRestriction);
            //TODO: CHECK UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a restriction from a product component
        /// </summary>
        /// <param name="deleteRestrictionFromProductComponentDTO">DeleteRestrictionFromProductComponentDTO with the restriction deletion information</param>
        public void deleteRestrictionFromProductComponent(DeleteRestrictionFromProductComponentModelView deleteRestrictionFromProductComponentDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithComponentBeingDeletedRestriction=productRepository.find(deleteRestrictionFromProductComponentDTO.productID);
            //TODO: CHECK PRODUCT EXISTENCE
            Product productComponentBeingDeletedRestriction=productRepository.find(deleteRestrictionFromProductComponentDTO.componentID);
            //TODO: CHECK COMPLEMENTED PRODUCT EXISTENCE
            //TODO: RESTRICTION REPOSITORY ? ? ? ? ? ? :\

            productRepository.update(productWithComponentBeingDeletedRestriction);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            throw new NotImplementedException();
        }


        /// <summary>
        /// Adds a restriction to a product width dimension
        /// </summary>
        /// <param name="addRestrictionToProductDimensionModelView">AddRestrictionToProductDimensionModelView with the restriction addition information</param>
        /// <returns>GetAllRestrictionsModelView with the updated dimension restrictions information</returns>
        public GetAllRestrictionsModelView addRestrictionToProductWidthDimension(AddRestrictionToProductComponentDimensionModelView addRestrictionToProductDimensionModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithDimensionBeingAddedRestriction=productRepository.find(addRestrictionToProductDimensionModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE

            //TODO: FINISH IMPLEMENTATION (@Moreira (1160928))
            productRepository.update(productWithDimensionBeingAddedRestriction);
            //TODO: CHECK UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a restriction to a product height dimension
        /// </summary>
        /// <param name="addRestrictionToProductDimensionModelView">AddRestrictionToProductDimensionModelView with the restriction addition information</param>
        /// <returns>GetAllRestrictionsModelView with the updated dimension restrictions information</returns>
        public GetAllRestrictionsModelView addRestrictionToProductHeightDimension(AddRestrictionToProductComponentDimensionModelView addRestrictionToProductDimensionModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithDimensionBeingAddedRestriction=productRepository.find(addRestrictionToProductDimensionModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE

            //TODO: FINISH IMPLEMENTATION (@Moreira (1160928))
            productRepository.update(productWithDimensionBeingAddedRestriction);
            //TODO: CHECK UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a restriction to a product depth dimension
        /// </summary>
        /// <param name="addRestrictionToProductDimensionModelView">AddRestrictionToProductDimensionModelView with the restriction addition information</param>
        /// <returns>GetAllRestrictionsModelView with the updated dimension restrictions information</returns>
        public GetAllRestrictionsModelView addRestrictionToProductDepthDimension(AddRestrictionToProductComponentDimensionModelView addRestrictionToProductDimensionModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithDimensionBeingAddedRestriction=productRepository.find(addRestrictionToProductDimensionModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE

            //TODO: FINISH IMPLEMENTATION (@Moreira (1160928))
            productRepository.update(productWithDimensionBeingAddedRestriction);
            //TODO: CHECK UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a restriction from a product component width dimension
        /// </summary>
        /// <param name="deleteRestrictionFromProductDimensionModelView">DeleteRestrictionFromProductDimensionModelView with the restriction deletion information</param>
        public void deleteRestrictionFromProductComponentWidthDimension(DeleteRestrictionFromProductComponentDimensionModelView deleteRestrictionFromProductDimensionModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithDimensionBeingDeletedRestriction=productRepository.find(deleteRestrictionFromProductDimensionModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE

            //TODO: FINISH IMPLEMENTATION (@Moreira (1160928))
            productRepository.update(productWithDimensionBeingDeletedRestriction);
            //TODO: CHECK UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a restriction from a product component height dimension
        /// </summary>
        /// <param name="deleteRestrictionFromProductDimensionModelView">DeleteRestrictionFromProductDimensionModelView with the restriction deletion information</param>
        public void deleteRestrictionFromProductComponentHeightDimension(DeleteRestrictionFromProductComponentDimensionModelView deleteRestrictionFromProductDimensionModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithDimensionBeingDeletedRestriction=productRepository.find(deleteRestrictionFromProductDimensionModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE

            //TODO: FINISH IMPLEMENTATION (@Moreira (1160928))
            productRepository.update(productWithDimensionBeingDeletedRestriction);
            //TODO: CHECK UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a restriction from a product component depth dimension
        /// </summary>
        /// <param name="deleteRestrictionFromProductDimensionModelView">DeleteRestrictionFromProductDimensionModelView with the restriction deletion information</param>
        public void deleteRestrictionFromProductComponentDepthDimension(DeleteRestrictionFromProductComponentDimensionModelView deleteRestrictionFromProductDimensionModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productWithDimensionBeingDeletedRestriction=productRepository.find(deleteRestrictionFromProductDimensionModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE

            //TODO: FINISH IMPLEMENTATION (@Moreira (1160928))
            productRepository.update(productWithDimensionBeingDeletedRestriction);
            //TODO: CHECK UPDATE SUCCESS
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disables a product
        /// </summary>
        /// <param name="productDTO">ProductDTO with the product data being disabled</param>
        /// <returns>boolean true if the product was disabled with success, false if not</returns>
        public bool disableProduct(ProductDTO productDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingDisabled=productRepository.find(productDTO.id);
            return productBeingDisabled!=null && productBeingDisabled.deactivate() && productRepository.update(productBeingDisabled)!=null;
        }

        /// <summary>
        /// Removes (Disables) a product
        /// </summary>
        /// <param name="productDTO">DTO with the product information</param>
        /// <returns>boolean true if the product was removed (disabled) with success</returns>
        public bool removeProduct(ProductDTO productDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingRemoved=productRepository.find(productDTO.id);
            return productBeingRemoved!=null && productBeingRemoved.deactivate() && productRepository.update(productBeingRemoved)!=null;
        }

        /// <summary>
        /// Fetches a list of all products present in the product repository
        /// </summary>
        /// <returns>a list of all of the products model views</returns>
        public GetAllProductsModelView findAllProducts(){
            return ProductModelViewService.fromCollection(
                PersistenceContext.repositories().createProductRepository().findAll()
            );
        }

        /// <summary>
        /// Returns a product which has a certain persistence id
        /// </summary>
        /// <param name="productDTO">FetchProductDTO with the product fetch information</param>
        /// <returns>GetProductModelView with the product which has a certain persistence id</returns>
        public GetProductModelView findProductByID(FetchProductDTO fetchProductDTO){
            return ProductModelViewService.fromEntity(
                PersistenceContext.repositories().createProductRepository().find(
                    fetchProductDTO.id
                ));
        }

        /// <summary>
        /// Returns a product which has a certain reference
        /// </summary>
        /// <param name="productDTO">FetchProductDTO with the product fetch information</param>
        /// <returns>GetProductModelView with the product which has a certain reference</returns>
        public GetProductModelView findByReference(FetchProductDTO fetchProductDTO){
            return ProductModelViewService.fromEntity(
                PersistenceContext.repositories().createProductRepository().find(
                    fetchProductDTO.reference
                ));
        }

        /// <summary>
        /// Adds a restriction to a product's component and returns the restriction's algorithm list of inputs
        /// </summary>
        /// <param name="productID">product's id</param>
        /// <param name="productComponentID">product's component id</param>
        /// <param name="restDTO">Data Transfer Object of the restriction to add</param>
        /// <returns>list of inputs for the restriction's algorithm</returns>
        public RestrictionDTO addComponentRestriction(long productID, long productComponentID, RestrictionDTO restDTO){
            if(Collections.isEnumerableNullOrEmpty(restDTO.inputs)){
                //gets required list of inputs for the algorithm
                List<Input> inputs=new AlgorithmFactory().createAlgorithm(restDTO.algorithm).getRequiredInputs();
                //if the algorithm did not need any inputs then persist
                if(Collections.isEnumerableNullOrEmpty(inputs)){
                    ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
                    Product product=productRepository.find(productID);
                    Product component=productRepository.find(productComponentID);
                    Restriction restriction=restDTO.toEntity();
                    product.addComponentRestriction(component, restriction);
                    productRepository.update(product);
                    return restDTO;
                }
                throw new ArgumentException(LIST_OF_INPUTS_MISSING);
            } else {
                ProductRepository productRepository = PersistenceContext.repositories().createProductRepository();
                Product product = productRepository.find(productID);
                Product component = productRepository.find(productComponentID);
                List<Input> inputs = new List<Input>(DTOUtils.reverseDTOS(restDTO.inputs));
                if(new AlgorithmFactory().createAlgorithm(restDTO.algorithm).isWithinDataRange(inputs)) {
                    Restriction restriction = restDTO.toEntity();
                    product.addComponentRestriction(component, restriction);
                    productRepository.update(product);
                }
                return restDTO;
            }
        }

        /// <summary>
        /// Returns an enumerable of dimensions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the dimensions found on a product DTO</returns>
        internal IEnumerable<Dimension> getProductDTOEnumerableDimensions(List<DimensionDTO> dimensionsDTOs){
            List<Dimension> dimensions=new List<Dimension>();
            foreach (DimensionDTO dimensionDTO in dimensionsDTOs){
                if(dimensionDTO.GetType() == typeof(DiscreteDimensionIntervalDTO)){
                    DiscreteDimensionIntervalDTO ddiDTO=(DiscreteDimensionIntervalDTO)dimensionDTO;
                    DiscreteDimensionInterval discreteInterval=new DiscreteDimensionInterval(ddiDTO.values);
                    dimensions.Add(discreteInterval);
                } else if(dimensionDTO.GetType() == typeof(ContinuousDimensionIntervalDTO)){
                    ContinuousDimensionIntervalDTO cdiDTO=(ContinuousDimensionIntervalDTO)dimensionDTO;
                    ContinuousDimensionInterval continuousInterval=new ContinuousDimensionInterval(cdiDTO.minValue, cdiDTO.maxValue, cdiDTO.increment);
                    dimensions.Add(continuousInterval);
                } else if(dimensionDTO.GetType() == typeof(SingleValueDimensionDTO)){
                    SingleValueDimensionDTO svdDTO=(SingleValueDimensionDTO)dimensionDTO;
                    SingleValueDimension dimensionValue=new SingleValueDimension(svdDTO.value);
                    dimensions.Add(dimensionValue);
                }
            }
            return dimensions;
        }

        /// <summary>
        /// Extracts an enumerable of products dto from an enumerable of components dto
        /// </summary>
        /// <param name="componentDTO">IEnumerable with the components dto</param>
        /// <returns>IEnumerable with the extracted products dto</returns>
        private IEnumerable<ProductDTO> extractProductsDTOFromComponentsDTO(IEnumerable<ComponentDTO> componentsDTO){
            List<ProductDTO> productsDTO=new List<ProductDTO>();
            foreach (ComponentDTO componentDTO in componentsDTO) productsDTO.Add(componentDTO.product);
            return productsDTO;
        }
    }

}