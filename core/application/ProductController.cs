using System;
using System.Collections.Generic;
using core.domain;
using core.persistence;
using core.dto;
using core.modelview.component;
using core.modelview.dimension;
using core.modelview.material;
using core.modelview.product;
using core.modelview.restriction;
using core.services;
using core.services.ensurance;
using support.dto;
using support.utils;

namespace core.application{
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController{
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
        /// <param name="productAsDTO">DTO with the product information</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public ProductDTO addProduct(ProductDTO productAsDTO){
            Product newProduct=new ProductDTOService().transform(productAsDTO);
            Product createdProduct=PersistenceContext.repositories().createProductRepository().save(newProduct);
            if (createdProduct == null) return null;
            return createdProduct.toDTO();
        }

        /// <summary>
        /// Updates basic information of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductBasicInformation(UpdateProductDTO updateProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated=productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess=true;
            bool perfomedAtLeastOneUpdate=false;
            if (updateProductDTO.reference!=null){
                updatedWithSuccess&=productBeingUpdated.changeProductReference(updateProductDTO.reference);
                perfomedAtLeastOneUpdate=true;
            }
            if (updateProductDTO.designation!=null){
                updatedWithSuccess&=productBeingUpdated.changeProductDesignation(updateProductDTO.designation);
                perfomedAtLeastOneUpdate=true;
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;
            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Updates the materials of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductMaterials(UpdateProductDTO updateProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            MaterialRepository materialRepository=PersistenceContext.repositories().createMaterialRepository();
            Product productBeingUpdated=productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess=true;
            bool perfomedAtLeastOneUpdate=false;

            if (updateProductDTO.materialsToAdd!=null){
                IEnumerable<Material> materialsBeingAdded=materialRepository.getMaterialsByIDS(updateProductDTO.materialsToAdd);
                FetchEnsurance.ensureMaterialsFetchWasSuccessful(updateProductDTO.materialsToAdd, materialsBeingAdded);
                foreach (Material material in materialsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addMaterial(material);
                perfomedAtLeastOneUpdate=true;
            }

            if (!updatedWithSuccess) return false;

            if (updateProductDTO.materialsToRemove!=null){
                IEnumerable<Material> materialsBeingRemoved=materialRepository.getMaterialsByIDS(updateProductDTO.materialsToRemove);
                FetchEnsurance.ensureMaterialsFetchWasSuccessful(updateProductDTO.materialsToRemove, materialsBeingRemoved);
                foreach (Material material in materialsBeingRemoved)
                    updatedWithSuccess&=productBeingUpdated.removeMaterial(material);
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
        /// Updates the components of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductComponents(UpdateProductDTO updateProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated=productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess=true;
            bool perfomedAtLeastOneUpdate=false;

            if (updateProductDTO.componentsToAdd!=null){
                IEnumerable<ProductDTO> productsDTO=extractProductsDTOFromComponentsDTO(updateProductDTO.componentsToAdd);
                IEnumerable<Product> complementedProducts=PersistenceContext.repositories().createProductRepository().fetchProductsByID(productsDTO);
                FetchEnsurance.ensureProductsFetchWasSuccesful(productsDTO, complementedProducts);
                foreach (Product complementedProduct in complementedProducts)
                    updatedWithSuccess&=productBeingUpdated.addComplementedProduct(complementedProduct);
                perfomedAtLeastOneUpdate=true;
            }

            if (!updatedWithSuccess) return false;


            if (updateProductDTO.componentsToRemove!=null){
                IEnumerable<ProductDTO> productsDTO=extractProductsDTOFromComponentsDTO(updateProductDTO.componentsToRemove);
                IEnumerable<Product> complementedProducts=PersistenceContext.repositories().createProductRepository().fetchProductsByID(productsDTO);
                FetchEnsurance.ensureProductsFetchWasSuccesful(productsDTO, complementedProducts);
                foreach (Product complementedProduct in complementedProducts)
                    updatedWithSuccess&=productBeingUpdated.removeComplementedProduct(complementedProduct);
                perfomedAtLeastOneUpdate=true;
            }

            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
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
        /// Updates the dimensions of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductDimensions(UpdateProductDTO updateProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated=productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess=true;
            bool perfomedAtLeastOneUpdate=false;
            if (updateProductDTO.dimensionsToAdd!=null && updateProductDTO.dimensionsToAdd.widthDimensionDTOs!=null){
                IEnumerable<Dimension> widthDimensionsBeingAdded=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.widthDimensionDTOs);
                foreach (Dimension widthDimension in widthDimensionsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addWidthDimension(widthDimension);
                perfomedAtLeastOneUpdate=true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToAdd!=null && updateProductDTO.dimensionsToAdd.heightDimensionDTOs!=null){
                IEnumerable<Dimension> heightDimensionsBeingAdded=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.heightDimensionDTOs);
                foreach (Dimension heightDimension in heightDimensionsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addHeightDimension(heightDimension);
                perfomedAtLeastOneUpdate=true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToAdd!=null && updateProductDTO.dimensionsToAdd.depthDimensionDTOs!=null){
                IEnumerable<Dimension> depthDimensionsBeingAdded=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.depthDimensionDTOs);
                foreach (Dimension depthDimension in depthDimensionsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addDepthDimension(depthDimension);
                perfomedAtLeastOneUpdate=true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToRemove!=null && updateProductDTO.dimensionsToRemove.widthDimensionDTOs!=null){
                IEnumerable<Dimension> widthDimensionsBeingRemoved=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToRemove.widthDimensionDTOs);
                if (widthDimensionsBeingRemoved!=null)
                    foreach (Dimension widthDimension in widthDimensionsBeingRemoved)
                        updatedWithSuccess&=productBeingUpdated.removeWidthDimension(widthDimension);
                perfomedAtLeastOneUpdate=true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToRemove!=null && updateProductDTO.dimensionsToRemove.heightDimensionDTOs!=null){
                IEnumerable<Dimension> heightDimensionsBeingRemoved=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToRemove.heightDimensionDTOs);
                if (heightDimensionsBeingRemoved!=null)
                    foreach (Dimension heightDimension in heightDimensionsBeingRemoved)
                        updatedWithSuccess&=productBeingUpdated.removeHeightDimension(heightDimension);
                perfomedAtLeastOneUpdate=true;
                if (!updatedWithSuccess) return false;
            }

            if (updateProductDTO.dimensionsToRemove!=null && updateProductDTO.dimensionsToRemove.depthDimensionDTOs!=null){
                IEnumerable<Dimension> depthDimensionsBeingRemoved=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToRemove.depthDimensionDTOs);
                if (depthDimensionsBeingRemoved!=null)
                    foreach (Dimension depthDimension in depthDimensionsBeingRemoved)
                        updatedWithSuccess&=productBeingUpdated.removeDepthDimension(depthDimension);
                perfomedAtLeastOneUpdate=true;
                if (!updatedWithSuccess) return false;
            }

            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;

            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Adds a width dimension to a product
        /// </summary>
        /// <param name="addDimensionToProductModelView">AddDimensionToProductModelView with the dimension addition information</param>
        /// <returns>GetAllDimensionsModelView with the updated collection of width dimensions of the product which dimension was added</returns>
        public GetAllDimensionsModelView addWidthDimensionToProduct(AddDimensionToProductModelView addDimensionToProductModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToAddWidthDimension=productRepository.find(addDimensionToProductModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE
            productToAddWidthDimension.addWidthDimension(addDimensionToProductModelView.widthDimension.toEntity());
            //TODO: CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToAddWidthDimension);
            return DimensionModelViewService.fromCollection(productToAddWidthDimension.widthValues);
        }

        /// <summary>
        /// Adds a height dimension to a product
        /// </summary>
        /// <param name="addDimensionToProductModelView">AddDimensionToProductModelView with the dimension addition information</param>
        /// <returns>GetAllDimensionsModelView with the updated collection of height dimensions of the product which dimension was added</returns>
        public GetAllDimensionsModelView addHeightDimensionToProduct(AddDimensionToProductModelView addDimensionToProductModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToAddHeightDimension=productRepository.find(addDimensionToProductModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE
            productToAddHeightDimension.addHeightDimension(addDimensionToProductModelView.heightDimension.toEntity());
            //TODO: CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToAddHeightDimension);
            return DimensionModelViewService.fromCollection(productToAddHeightDimension.heightValues);
        }

        /// <summary>
        /// Adds a depth dimension to a product
        /// </summary>
        /// <param name="addDimensionToProductModelView">AddDimensionToProductModelView with the dimension addition information</param>
        /// <returns>GetAllDimensionsModelView with the updated collection of depth dimensions of the product which dimension was added</returns>
        public GetAllDimensionsModelView addDepthDimensionToProduct(AddDimensionToProductModelView addDimensionToProductModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToAddDepthDimension=productRepository.find(addDimensionToProductModelView.productID);
            //TODO: CHECK PRODUCT EXISTENCE
            productToAddDepthDimension.addDepthDimension(addDimensionToProductModelView.depthDimension.toEntity());
            //TODO: CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToAddDepthDimension);
            return DimensionModelViewService.fromCollection(productToAddDepthDimension.depthValues);
        }

        /// <summary>
        /// Deletes a width dimension from a product
        /// </summary>
        /// <param name="deleteDimensionFromProductModelView">DeleteDimensionFromProductModelView with the dimension deletion information</param>
        public void deleteWidthDimensionFromProduct(DeleteDimensionFromProductModelView deleteDimensionFromProductModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToRemoveDimension=productRepository.find(deleteDimensionFromProductModelView.productID);
            //TODO:CHECK PRODUCT EXISTENCE
            FetchProductDimensionDTO fetchProductDimensionDTO=new FetchProductDimensionDTO();
            fetchProductDimensionDTO.productID=deleteDimensionFromProductModelView.productID;
            fetchProductDimensionDTO.dimensionID=deleteDimensionFromProductModelView.widthDimensionID;
            Dimension widthDimension=productRepository.fetchProductWidthDimension(fetchProductDimensionDTO);
            //TODO:CHECK WIDTH DIMENSION EXISTENCE
            productToRemoveDimension.removeWidthDimension(widthDimension);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToRemoveDimension);
        }

        /// <summary>
        /// Deletes a height dimension from a product
        /// </summary>
        /// <param name="deleteDimensionFromProductModelView">DeleteDimensionFromProductModelView with the dimension deletion information</param>
        public void deleteHeightDimensionFromProduct(DeleteDimensionFromProductModelView deleteDimensionFromProductModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToRemoveDimension=productRepository.find(deleteDimensionFromProductModelView.productID);
            //TODO:CHECK PRODUCT EXISTENCE
            FetchProductDimensionDTO fetchProductDimensionDTO=new FetchProductDimensionDTO();
            fetchProductDimensionDTO.productID=deleteDimensionFromProductModelView.productID;
            fetchProductDimensionDTO.dimensionID=deleteDimensionFromProductModelView.heightDimensionID;
            Dimension heightDimension=productRepository.fetchProductHeightDimension(fetchProductDimensionDTO);
            //TODO:CHECK HEIGHT DIMENSION EXISTENCE
            productToRemoveDimension.removeHeightDimension(heightDimension);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToRemoveDimension);
        }

        /// <summary>
        /// Deletes a depth dimension from a product
        /// </summary>
        /// <param name="deleteDimensionFromProductModelView">DeleteDimensionFromProductModelView with the dimension deletion information</param>
        public void deleteDepthDimensionFromProduct(DeleteDimensionFromProductModelView deleteDimensionFromProductModelView){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productToRemoveDimension=productRepository.find(deleteDimensionFromProductModelView.productID);
            //TODO:CHECK PRODUCT EXISTENCE
            FetchProductDimensionDTO fetchProductDimensionDTO=new FetchProductDimensionDTO();
            fetchProductDimensionDTO.productID=deleteDimensionFromProductModelView.productID;
            fetchProductDimensionDTO.dimensionID=deleteDimensionFromProductModelView.depthDimensionID;
            Dimension depthDimension=productRepository.fetchProductDepthDimension(fetchProductDimensionDTO);
            //TODO:CHECK DEPTH DIMENSION EXISTENCE
            productToRemoveDimension.removeDepthDimension(depthDimension);
            //TODO:CHECK PRODUCT UPDATE SUCCESS
            productRepository.update(productToRemoveDimension);
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
        /// Updates the category of a product
        /// </summary>
        /// <param name="updateProductDTO">UpdateProductDTO with the data regarding the product update</param>
        /// <returns>boolean true if the update was successful, fasle if not</returns>
        public bool updateProductCategory(UpdateProductDTO updateProductDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingUpdated=productRepository.find(updateProductDTO.id);
            bool updatedWithSuccess=true;
            bool perfomedAtLeastOneUpdate=false;
            if (updateProductDTO.productCategoryToUpdate!=null){
                ProductCategory productCategory=PersistenceContext.repositories().createProductCategoryRepository().find(updateProductDTO.productCategoryToUpdate.id);
                updatedWithSuccess&=productBeingUpdated.changeProductCategory(productCategory);
                perfomedAtLeastOneUpdate=true;
            }
            if (!perfomedAtLeastOneUpdate || !updatedWithSuccess) return false;
            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
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

        public bool defineProductDimensions(ProductDTO productDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product product=productRepository.find(productDTO.id);

            IEnumerable<Dimension> heightDimensions=getProductDTOEnumerableDimensions(productDTO.dimensions.heightDimensionDTOs);
            IEnumerable<Dimension> widthDimensions=getProductDTOEnumerableDimensions(productDTO.dimensions.widthDimensionDTOs);
            IEnumerable<Dimension> depthDimensions=getProductDTOEnumerableDimensions(productDTO.dimensions.depthDimensionDTOs);

            foreach (Dimension heightDimension in heightDimensions){ if (!product.addHeightDimension(heightDimension)) return false; }
            foreach (Dimension widthDimension in widthDimensions){ if (!product.addWidthDimension(widthDimension)) return false; }
            foreach (Dimension depthDimension in depthDimensions){ if (!product.addDepthDimension(depthDimension)) return false; }

            return productRepository.save(product)!=null;
        }

        /// <summary>
        /// Adds a restriction to a product's component and returns the restriction's algorithm list of inputs
        /// </summary>
        /// <param name="productID">product's id</param>
        /// <param name="productComponentID">product's component id</param>
        /// <param name="restDTO">Data Transfer Object of the restriction to add</param>
        /// <returns>list of inputs for the restriction's algorithm</returns>
        public RestrictionDTO addComponentRestriction(long productID, long productComponentID, RestrictionDTO restDTO){
            if (Collections.isEnumerableNullOrEmpty(restDTO.inputs)){
                //gets required list of inputs for the algorithm
                List<Input> inputs=new AlgorithmFactory().createAlgorithm(restDTO.algorithm).getRequiredInputs();
                //if the algorithm did not need any inputs then persist
                if (Collections.isEnumerableNullOrEmpty(inputs)){
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
                if (new AlgorithmFactory().createAlgorithm(restDTO.algorithm).isWithinDataRange(inputs)) {
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
                if (dimensionDTO.GetType() == typeof(DiscreteDimensionIntervalDTO)){
                    DiscreteDimensionIntervalDTO ddiDTO=(DiscreteDimensionIntervalDTO)dimensionDTO;
                    DiscreteDimensionInterval discreteInterval=new DiscreteDimensionInterval(ddiDTO.values);
                    dimensions.Add(discreteInterval);
                } else if (dimensionDTO.GetType() == typeof(ContinuousDimensionIntervalDTO)){
                    ContinuousDimensionIntervalDTO cdiDTO=(ContinuousDimensionIntervalDTO)dimensionDTO;
                    ContinuousDimensionInterval continuousInterval=new ContinuousDimensionInterval(cdiDTO.minValue, cdiDTO.maxValue, cdiDTO.increment);
                    dimensions.Add(continuousInterval);
                } else if (dimensionDTO.GetType() == typeof(SingleValueDimensionDTO)){
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