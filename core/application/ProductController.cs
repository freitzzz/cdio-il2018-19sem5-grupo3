using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.persistence;
using core.dto;
using core.services;

namespace core.application {
    /// <summary>
    /// Core ProductController class
    /// </summary>
    public class ProductController {
        /// <summary>
        /// Builds a new ProductController
        /// </summary>
        public ProductController() {}

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="productAsDTO">DTO with the product information</param>
        /// <returns>DTO with the created product DTO, null if the product was not created</returns>
        public ProductDTO addProduct(ProductDTO productAsDTO) {
            Product newProduct=new ProductDTOService().transform(productAsDTO);
            Product createdProduct=PersistenceContext.repositories().createProductRepository().save(newProduct);
            if (createdProduct==null) return null;
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
            if(updateProductDTO.reference!=null){
                updatedWithSuccess&=productBeingUpdated.changeProductReference(updateProductDTO.reference);
                perfomedAtLeastOneUpdate=true;
            }
            if(updateProductDTO.designation!=null){
                updatedWithSuccess&=productBeingUpdated.changeProductDesignation(updateProductDTO.designation);
                perfomedAtLeastOneUpdate=true;
            }
            if(!perfomedAtLeastOneUpdate||!updatedWithSuccess)return false;
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
            
            if(updateProductDTO.materialsToAdd!=null){
                IEnumerable<Material> materialsBeingAdded=materialRepository.getMaterialsByIDS(updateProductDTO.materialsToAdd);
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                foreach(Material material in materialsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addMaterial(material);
                perfomedAtLeastOneUpdate=true;
            }

            if(!updatedWithSuccess)return false;

            if(updateProductDTO.materialsToRemove!=null){
                IEnumerable<Material> materialsBeingRemoved=materialRepository.getMaterialsByIDS(updateProductDTO.materialsToRemove);
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                foreach(Material material in materialsBeingRemoved)
                    updatedWithSuccess&=productBeingUpdated.removeMaterial(material);
                perfomedAtLeastOneUpdate=true;
            }

            if(!perfomedAtLeastOneUpdate||!updatedWithSuccess)return false;

            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
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


            //TODO:DISCUSSION REGARDING COMPONENTS


            if(updateProductDTO.componentsToAdd!=null){
                IEnumerable<Component> componentsBeingAdded=null;
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                foreach(Component component in componentsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addComplementedProduct(component);
                perfomedAtLeastOneUpdate=true;
            }

            if(!updatedWithSuccess)return false;

            if(updateProductDTO.componentsToRemove!=null){
                IEnumerable<Component> componentsBeingRemoved=null;
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                foreach(Component component in componentsBeingRemoved)
                    updatedWithSuccess&=productBeingUpdated.removeComplementedProduct(component);
                perfomedAtLeastOneUpdate=true;
            }

            if(!perfomedAtLeastOneUpdate||!updatedWithSuccess)return false;

            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
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
            if(updateProductDTO.dimensionsToAdd.widthDimensionDTOs!=null){
                IEnumerable<Dimension> widthDimensionsBeingAdded=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.widthDimensionDTOs);
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                foreach(Dimension widthDimension in widthDimensionsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addWidthDimension(widthDimension);
                perfomedAtLeastOneUpdate=true;
                if(!updatedWithSuccess)return false;
            }

            if(updateProductDTO.dimensionsToAdd.heightDimensionDTOs!=null){
                IEnumerable<Dimension> heightDimensionsBeingAdded=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.heightDimensionDTOs);
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                foreach(Dimension heightDimension in heightDimensionsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addHeightDimension(heightDimension);
                perfomedAtLeastOneUpdate=true;
                if(!updatedWithSuccess)return false;
            }

            if(updateProductDTO.dimensionsToAdd.depthDimensionDTOs!=null){
                IEnumerable<Dimension> depthDimensionsBeingAdded=DTOUtils.reverseDTOS(updateProductDTO.dimensionsToAdd.depthDimensionDTOs);
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                foreach(Dimension depthDimension in depthDimensionsBeingAdded)
                    updatedWithSuccess&=productBeingUpdated.addDepthDimension(depthDimension);
                perfomedAtLeastOneUpdate=true;
                if(!updatedWithSuccess)return false;
            }

            
            //TODO:FIX DIMENSIONS RETRIEVAL


            if(updateProductDTO.dimensionsToRemove.widthDimensionDTOs!=null){
                IEnumerable<Dimension> widthDimensionsBeingRemoved=null;
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                if(widthDimensionsBeingRemoved!=null)
                    foreach(Dimension widthDimension in widthDimensionsBeingRemoved)
                        updatedWithSuccess&=productBeingUpdated.removeWidthDimension(widthDimension);
               perfomedAtLeastOneUpdate=true;
                if(!updatedWithSuccess)return false;
            }

            if(updateProductDTO.dimensionsToRemove.heightDimensionDTOs!=null){
                IEnumerable<Dimension> heightDimensionsBeingRemoved=null;
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                if(heightDimensionsBeingRemoved!=null)
                    foreach(Dimension heightDimension in heightDimensionsBeingRemoved)
                        updatedWithSuccess&=productBeingUpdated.removeHeightDimension(heightDimension);
                perfomedAtLeastOneUpdate=true;
                if(!updatedWithSuccess)return false;
            }

            if(updateProductDTO.dimensionsToRemove.depthDimensionDTOs!=null){
                IEnumerable<Dimension> depthDimensionsBeingRemoved=null;
                //TODO:CHECK DTO AND ENTITY LISTS LENGTH
                if(depthDimensionsBeingRemoved!=null)
                    foreach(Dimension depthDimension in depthDimensionsBeingRemoved)
                        updatedWithSuccess&=productBeingUpdated.removeDepthDimension(depthDimension);
                perfomedAtLeastOneUpdate=true;
                if(!updatedWithSuccess)return false;
            }

            if(!perfomedAtLeastOneUpdate||!updatedWithSuccess)return false;

            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
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
            if(updateProductDTO.productCategoryToUpdate!=null){
                ProductCategory productCategory=PersistenceContext.repositories().createProductCategoryRepository().find(updateProductDTO.productCategoryToUpdate.id);
                updatedWithSuccess&=productBeingUpdated.changeProductCategory(productCategory);
                perfomedAtLeastOneUpdate=true;
            }
            if(!perfomedAtLeastOneUpdate||!updatedWithSuccess)return false;
            updatedWithSuccess&=productRepository.update(productBeingUpdated)!=null;
            return updatedWithSuccess;
        }

        /// <summary>
        /// Disables a product
        /// </summary>
        /// <param name="productDTO">ProductDTO with the product data being disabled</param>
        /// <returns>boolean true if the product was disabled with success, false if not</returns>
        public bool disableProduct(ProductDTO productDTO){
            Product productBeingDisabled=PersistenceContext.repositories().createProductRepository().find(productDTO.id);
            return productBeingDisabled!=null && productBeingDisabled.disable();
        }

        /// <summary>
        /// Removes (Disables) a product
        /// </summary>
        /// <param name="productDTO">DTO with the product information</param>
        /// <returns>boolean true if the product was removed (disabled) with success</returns>
        public bool removeProduct(ProductDTO productDTO) {
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product productBeingRemoved = productRepository.find(productDTO.id);
            return productBeingRemoved != null && productBeingRemoved.disable() && productRepository.update(productBeingRemoved) != null;
        }

        /// <summary>
        /// Fetches a list of all products present in the product repository
        /// </summary>
        /// <returns>a list of all of the products DTOs</returns>
        public List<ProductDTO> findAllProducts() {
            List<ProductDTO> productDTOList = new List<ProductDTO>();

            IEnumerable<Product> productList = PersistenceContext.repositories().createProductRepository().findAll();

            if (productList == null||!productList.GetEnumerator().MoveNext()) {
                return null;
            }

            foreach (Product product in productList) {
                productDTOList.Add(product.toDTO());
            }

            return productDTOList;
        }

        /// <summary>
        /// Returns a product which has a certain persistence id
        /// </summary>
        /// <param name="productDTO">ProductDTO with the product information</param>
        /// <returns>ProductDTO with the product which has a certain persistence id</returns>
        public ProductDTO findProductByID(ProductDTO productDTO) {
            return PersistenceContext.repositories().createProductRepository().find(productDTO.id).toDTO();
        }

        public ProductDTO findByReference(string reference) {
            return PersistenceContext.repositories().createProductRepository().find(reference).toDTO();
        }

        public bool defineProductDimensions(ProductDTO productDTO){
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product product = productRepository.find(productDTO.id);
            
            IEnumerable<Dimension> heightDimensions = getProductDTOEnumerableDimensions(productDTO.dimensions.heightDimensionDTOs);
            IEnumerable<Dimension> widthDimensions = getProductDTOEnumerableDimensions(productDTO.dimensions.widthDimensionDTOs);
            IEnumerable<Dimension> depthDimensions = getProductDTOEnumerableDimensions(productDTO.dimensions.depthDimensionDTOs);

            foreach(Dimension heightDimension in heightDimensions){if(!product.addHeightDimension(heightDimension)) return false;}
            foreach(Dimension widthDimension in widthDimensions){if(!product.addWidthDimension(widthDimension)) return false;}
            foreach(Dimension depthDimension in depthDimensions){if(!product.addDepthDimension(depthDimension)) return false;}

            return productRepository.save(product) != null;
        }

        /// <summary>
        /// Updates a product with new dimensions and/or new materials
        /// </summary>
        /// <param name="updatesDTO"></param>
        /// <returns></returns>
        /// TODO Refactor method 
        public ProductDTO updateProduct(ProductDTO updatesDTO) {
            ProductRepository productRepository=PersistenceContext.repositories().createProductRepository();
            Product oldProduct = productRepository.find(updatesDTO.id);
            if (oldProduct == null) {
                return null;
            }

            IEnumerable<Dimension> heightDimensions = getProductDTOEnumerableDimensions(updatesDTO.dimensions.heightDimensionDTOs);
            IEnumerable<Dimension> widthDimensions = getProductDTOEnumerableDimensions(updatesDTO.dimensions.widthDimensionDTOs);
            IEnumerable<Dimension> depthDimensions = getProductDTOEnumerableDimensions(updatesDTO.dimensions.depthDimensionDTOs);

            foreach (Dimension heightDimension in heightDimensions) { if (!oldProduct.addHeightDimension(heightDimension)) return null; }
            foreach (Dimension widthDimension in widthDimensions) { if (!oldProduct.addWidthDimension(widthDimension)) return null; }
            foreach (Dimension depthDimension in depthDimensions) { if (!oldProduct.addDepthDimension(depthDimension)) return null; }
            //addMaterials(updatesDTO, oldProduct);

            Product prod = productRepository.update(oldProduct);
            if(prod == null) {
                return null;
            } else {
                return prod.toDTO();
            }
        }

        /// <summary>
        /// Returns an enumerable of dimensions found on a product DTO
        /// </summary>
        /// <param name="productDTO">DTO with the product DTO</param>
        /// <returns>IEnumerable with the dimensions found on a product DTO</returns>
        internal IEnumerable<Dimension> getProductDTOEnumerableDimensions(List<DimensionDTO> dimensionsDTOs) {
            List<Dimension> dimensions = new List<Dimension>();
            foreach (DimensionDTO dimensionDTO in dimensionsDTOs) {
                if (dimensionDTO.GetType() == typeof(DiscreteDimensionIntervalDTO)) {
                    DiscreteDimensionIntervalDTO ddiDTO = (DiscreteDimensionIntervalDTO)dimensionDTO;
                    DiscreteDimensionInterval discreteInterval = DiscreteDimensionInterval.valueOf(ddiDTO.values);
                    dimensions.Add(discreteInterval);
                } else if (dimensionDTO.GetType() == typeof(ContinuousDimensionIntervalDTO) ) {
                    ContinuousDimensionIntervalDTO cdiDTO = (ContinuousDimensionIntervalDTO)dimensionDTO;
                    ContinuousDimensionInterval continuousInterval = ContinuousDimensionInterval.valueOf(cdiDTO.minValue, cdiDTO.maxValue, cdiDTO.increment);
                    dimensions.Add(continuousInterval);
                } else if(dimensionDTO.GetType() == typeof(SingleValueDimensionDTO)){
                    SingleValueDimensionDTO svdDTO = (SingleValueDimensionDTO)dimensionDTO;
                    SingleValueDimension dimensionValue = SingleValueDimension.valueOf(svdDTO.value);
                    dimensions.Add(dimensionValue);
                }
            }
            return dimensions;
        }

        /// <summary>
        /// Adds new materials to updated product
        /// </summary>
        /// <param name="productDTO">list of updates in DTO</param>
        /// <param name="oldProduct">product to be updated</param>
        /// TODO:REFACTOR THIS 
        /* private void addMaterials(ProductDTO productDTO, Product oldProduct) {
            foreach (ProductMaterialDTO prodMDTO in productDTO.productMaterials) {
                long matID = prodMDTO.material.id;
                oldProduct.addMaterial(materialRepository.find(matID));
            }
        } */

        /// <summary>
        /// Parses an enumerable of products persistence identifiers as an enumerable of entities
        /// </summary>
        /// <param name="productsIDS">Enumerable with the products persistence identifiers</param>
        /// <returns>IEnumerable with the products ids as entities</returns>
        internal IEnumerable<Product> enumerableOfProductsIDSAsEntities(IEnumerable<long> productsIDS) {
            if (productsIDS == null) return null;
            List<Product> products = new List<Product>();
            IEnumerator<long> productsIDSIterator = productsIDS.GetEnumerator();
            long nextProductID = productsIDSIterator.Current;
            while (productsIDSIterator.MoveNext()) {
                nextProductID = productsIDSIterator.Current;
                products.Add(PersistenceContext.repositories().createProductRepository().find(nextProductID));
            }
            return products;
        }

    }

}